using UnityEngine;

public class PlacedMachine
{
    public MachineData data;
    public ItemData sourceItem; // ItemData consumed on placement — returned on pickup
    public Vector2Int cell;
    public GameObject gameObject;
    public RecipeData activeRecipe;
    public MachineInstance instance;
}
