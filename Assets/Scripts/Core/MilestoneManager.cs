using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviour
{
    public static MilestoneManager Instance { get; private set; }

    public static event System.Action<MilestoneData> OnMilestoneUnlocked;

    [SerializeField] private List<MilestoneData> allMilestones = new();

    private readonly HashSet<MilestoneData> _unlocked = new();

    public IReadOnlyList<MilestoneData> AllMilestones => allMilestones;
    public bool IsUnlocked(MilestoneData m) => _unlocked.Contains(m);

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        MachineInstance.OnItemProduced += HandleItemProduced;
        GridManager.OnMachinePlaced   += HandleMachinePlaced;

        // Fire any milestone with no trigger (auto-fires on scene load, e.g. S0)
        foreach (var m in allMilestones)
            if (m.triggerItem == null && m.triggerMachine == null)
                TryUnlock(m);
    }

    private void OnDestroy()
    {
        MachineInstance.OnItemProduced -= HandleItemProduced;
        GridManager.OnMachinePlaced   -= HandleMachinePlaced;
    }

    private void HandleItemProduced(ItemData item, MachineData machine)
    {
        foreach (var m in allMilestones)
            if (!_unlocked.Contains(m) && m.triggerItem == item)
                TryUnlock(m);
    }

    private void HandleMachinePlaced(PlacedMachine placed)
    {
        foreach (var m in allMilestones)
            if (!_unlocked.Contains(m) && m.triggerMachine == placed.data)
                TryUnlock(m);
    }

    private void TryUnlock(MilestoneData m)
    {
        if (_unlocked.Contains(m)) return;

        foreach (var pre in m.prerequisites)
            if (!_unlocked.Contains(pre)) return;

        _unlocked.Add(m);
        OnMilestoneUnlocked?.Invoke(m);
        Debug.Log($"[Milestone] Unlocked: {m.milestoneName}");

        // Cascade: re-check auto-milestones whose prerequisites may now be satisfied
        foreach (var other in allMilestones)
            if (!_unlocked.Contains(other) && other.triggerItem == null && other.triggerMachine == null)
                TryUnlock(other);
    }
}
