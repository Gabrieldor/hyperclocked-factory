using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "HF/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int stackSize = 64;

    [Header("Placement")]
    [Tooltip("If set, tapping a grid cell with this item places this machine. Leave null for non-placeable items (ores, dusts, ingots).")]
    public MachineData placeableMachine;
}
