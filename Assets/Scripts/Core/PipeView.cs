using UnityEngine;

/// Renders a pipe cell using the 12-frame bitmask sprite sheet.
///
/// Bitmask bits: N=0, E=1, S=2, W=3  (value = N*1 + E*2 + S*4 + W*8)
/// Maps all 16 bitmask combinations to the 12 sprite indices below.
///
/// Sprite sheet layout (0-indexed):
///   0  = N+S straight          8  = W+N+E tri  (missing S)
///   1  = E+W straight          9  = N+W+S tri  (missing E)
///   2  = S+E corner           10  = cross (N+E+S+W)
///   3  = S+W corner           11  = empty
///   4  = N+E corner
///   5  = N+W corner
///   6  = W+S+E tri (missing N)
///   7  = N+E+S tri (missing W)
public class PipeView : MonoBehaviour
{
    // bitmask index → sprite array index
    private static readonly int[] BitmaskToSprite = new int[16]
    {
         1, // 0000 no connections      → E+W straight (isolated pipe default)
         0, // 0001 N only             → N+S straight (end-cap fallback)
         1, // 0010 E only             → E+W straight (end-cap fallback)
         4, // 0011 N+E               → N+E corner
         0, // 0100 S only             → N+S straight (end-cap fallback)
         0, // 0101 N+S               → N+S straight
         2, // 0110 S+E               → S+E corner
         7, // 0111 N+E+S             → tri missing W
         1, // 1000 W only             → E+W straight (end-cap fallback)
         5, // 1001 N+W               → N+W corner
         1, // 1010 E+W               → E+W straight
         8, // 1011 N+E+W             → tri missing S
         3, // 1100 S+W               → S+W corner
         9, // 1101 N+S+W             → tri missing E
         6, // 1110 S+E+W             → tri missing N
        10, // 1111 N+E+S+W           → cross
    };

    private static Sprite[] _itemSprites;
    private static Sprite[] _fluidSprites;

    private SpriteRenderer _sr;
    private PipeLayer _layer;

    public void Build(PipeColor color, PipeLayer layer)
    {
        _layer = layer;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sortingOrder = 2;

        EnsureSpritesLoaded();
        // Default to E+W horizontal (matches hotbar icon and looks right when freshly placed)
        UpdateConnections(false, true, false, true);
    }

    public void UpdateConnections(bool n, bool e, bool s, bool w)
    {
        int mask = (n ? 1 : 0) | (e ? 2 : 0) | (s ? 4 : 0) | (w ? 8 : 0);
        int spriteIdx = BitmaskToSprite[mask];

        var sprites = _layer == PipeLayer.Fluid ? _fluidSprites : _itemSprites;
        if (sprites != null && spriteIdx < sprites.Length && sprites[spriteIdx] != null)
            _sr.sprite = sprites[spriteIdx];
        else
            _sr.sprite = Fallback();
    }

    // ── Sprite loading ────────────────────────────────────────────────────────

    private static void EnsureSpritesLoaded()
    {
        if (_itemSprites  == null) _itemSprites  = LoadSheet("Art/Pipes/pipes_item",  "item");
        if (_fluidSprites == null) _fluidSprites = LoadSheet("Art/Pipes/pipes_fluid", "fluid");
    }

    private static Sprite[] LoadSheet(string resourcePath, string layer)
    {
        var all = Resources.LoadAll<Sprite>(resourcePath);
        var result = new Sprite[12];
        foreach (var sp in all)
        {
            if (sp.name.StartsWith($"pipe_{layer}_"))
            {
                var suffix = sp.name.Substring($"pipe_{layer}_".Length);
                if (int.TryParse(suffix, out int idx) && idx < 12)
                    result[idx] = sp;
            }
        }
        return result;
    }

    // ── Fallback sprites ──────────────────────────────────────────────────────

    private static Sprite _fallback;
    private static Sprite Fallback()
    {
        if (_fallback != null) return _fallback;
        var tex = new Texture2D(32, 32);
        var px = new Color[32 * 32];
        for (int i = 0; i < px.Length; i++) px[i] = new Color(0.7f, 0.45f, 0.2f);
        tex.SetPixels(px); tex.Apply();
        _fallback = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32f);
        return _fallback;
    }

}
