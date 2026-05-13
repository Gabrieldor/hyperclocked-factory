using UnityEngine;

public enum PipeColor { White, Red, Green, Blue, Yellow }
public enum PipeLayer { Item, Fluid }

[CreateAssetMenu(fileName = "New_Pipe", menuName = "HF/Pipe Data")]
public class PipeData : ScriptableObject
{
    public string pipeName;
    public PipeColor color;
    public PipeLayer layer;
    public Sprite icon;
}
