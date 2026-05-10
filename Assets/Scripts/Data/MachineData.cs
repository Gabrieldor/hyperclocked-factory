using UnityEngine;

[CreateAssetMenu(fileName = "New_Machine", menuName = "HF/Machine Data")]
public class MachineData : ScriptableObject
{
    public string machineName;
    public Tier tier;
    public int tileSizeX = 1;
    public int tileSizeY = 1;
    public float steamPerTick;   // L/s; 0 if electric
    public float wattsDraw;      // W; 0 if steam-powered
    public int voltageRequired;  // V; 0 if not electric (32 / 128 / 512)
    public RecipeData[] availableRecipes;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
}
