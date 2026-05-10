using UnityEngine;

[System.Serializable]
public struct ItemStack
{
    public ItemData item;
    public int quantity;
}

[CreateAssetMenu(fileName = "New_Recipe", menuName = "HF/Recipe Data")]
public class RecipeData : ScriptableObject
{
    public ItemStack[] inputs;
    public ItemStack[] outputs;
    public float processingTime = 4f;
    public float energyCost;    // W (0 = steam-powered or manual)
    public float steamPerTick;  // L/s (0 = electric)
}
