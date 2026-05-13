using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneNodeUI : MonoBehaviour
{
    [SerializeField] private Image      background;
    [SerializeField] private TMP_Text   nameLabel;
    [SerializeField] private TMP_Text   conditionLabel;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text   rewardLabel;

    private MilestoneData _data;

    private static readonly Color LockedColor    = new Color(0.22f, 0.22f, 0.26f, 1f);
    private static readonly Color AvailableColor = new Color(0.75f, 0.75f, 0.80f, 1f);
    private static readonly Color UnlockedColor  = new Color(0.20f, 0.65f, 0.30f, 1f);

    public void Init(MilestoneData data)
    {
        _data = data;
        nameLabel.text      = data.milestoneName;
        conditionLabel.text = GetConditionText(data);
        rewardLabel.text    = GetRewardText(data);
        detailPanel.SetActive(false);
        Refresh();
    }

    public void Refresh()
    {
        if (_data == null) return;
        var mgr = MilestoneManager.Instance;
        if (mgr == null) return;

        bool unlocked  = mgr.IsUnlocked(_data);
        bool available = !unlocked && PrerequisitesMet(_data, mgr);

        background.color = unlocked ? UnlockedColor : available ? AvailableColor : LockedColor;

        var nameColor = (unlocked || available)
            ? new Color(0.95f, 0.95f, 0.95f)
            : new Color(0.45f, 0.45f, 0.45f);
        nameLabel.color      = nameColor;
        conditionLabel.color = nameColor;
    }

    public void OnTap() => detailPanel.SetActive(!detailPanel.activeSelf);

    private static bool PrerequisitesMet(MilestoneData m, MilestoneManager mgr)
    {
        foreach (var pre in m.prerequisites)
            if (!mgr.IsUnlocked(pre)) return false;
        return true;
    }

    private static string GetConditionText(MilestoneData m)
    {
        if (m.triggerItem    != null) return $"Produce: {m.triggerItem.itemName}";
        if (m.triggerMachine != null) return $"Build: {m.triggerMachine.machineName}";
        return "Automatic";
    }

    private static string GetRewardText(MilestoneData m)
    {
        return m.unlockType switch
        {
            MilestoneUnlockType.Machine        => $"Unlocks: {(m.unlockTarget != null ? m.unlockTarget.name : "machine")}",
            MilestoneUnlockType.Recipe         => $"Unlocks recipe: {(m.unlockTarget != null ? m.unlockTarget.name : "recipe")}",
            MilestoneUnlockType.FloorExpansion => $"Expands floor to {m.floorExpansionSize}×{m.floorExpansionSize}",
            MilestoneUnlockType.TierTransition => "Unlocks next tier",
            _                                  => "(no reward data)"
        };
    }
}
