using UnityEngine;
using UnityEngine.Tilemaps;

/// Custom tile that deterministically picks a sprite variant and rotation from
/// the cell position, giving visual variety without runtime randomness or extra packages.
[CreateAssetMenu(fileName = "RandomFloorTile", menuName = "HF/Random Floor Tile")]
public class RandomFloorTile : TileBase
{
    public Sprite[] variants;   // assign the sliced 32×32 sprites here

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (variants == null || variants.Length == 0) return;

        int hash = Hash(position.x, position.y);

        tileData.sprite = variants[Mathf.Abs(hash) % variants.Length];
        tileData.flags  = TileFlags.LockTransform;

        // Random 90° rotation seeded by position
        int rotIndex = Mathf.Abs(hash >> 8) % 4;
        float angle  = rotIndex * 90f;
        tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), Vector3.one);
    }

    // Simple integer hash — deterministic, fast, good enough for visual variation
    private static int Hash(int x, int y)
    {
        int h = x * 374761393 + y * 668265263;
        h = (h ^ (h >> 13)) * 1274126177;
        return h ^ (h >> 16);
    }
}
