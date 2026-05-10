using UnityEngine;

public enum MilestoneUnlockType { Machine, Recipe, FloorExpansion, TierTransition }

[CreateAssetMenu(fileName = "New_Milestone", menuName = "HF/Milestone Data")]
public class MilestoneData : ScriptableObject
{
    public string milestoneName;

    [Header("Trigger — fill one")]
    public ItemData triggerItem;
    public MachineData triggerMachine;

    [Header("Unlock")]
    public MilestoneUnlockType unlockType;
    public ScriptableObject unlockTarget;   // MachineData, RecipeData, or null for expansion/tier
    public int floorExpansionSize;          // used when unlockType == FloorExpansion

    [Header("Prerequisites")]
    public MilestoneData[] prerequisites;
}
