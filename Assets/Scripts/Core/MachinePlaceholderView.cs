using UnityEngine;

/// Placeholder visual for a placed machine. Swap out sprite + animator when real art lands.
[RequireComponent(typeof(SpriteRenderer))]
public class MachinePlaceholderView : MonoBehaviour
{
    // Tier tint colors — placeholder only, replace with real sprites
    private static readonly Color[] TierColors =
    {
        new Color(0.76f, 0.60f, 0.42f), // Steam  — bronze-ish
        new Color(0.55f, 0.55f, 0.60f), // LV     — steel grey
        new Color(0.40f, 0.70f, 0.85f), // MV     — aluminium blue
        new Color(0.95f, 0.80f, 0.20f), // HV     — gold
    };

    private SpriteRenderer _sr;

    private void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void Init(MachineData data)
    {
        if (data == null) return;

        if (data.sprite != null)
        {
            _sr.sprite = data.sprite;
        }
        else
        {
            _sr.sprite = CreatePlaceholderSprite();
            _sr.color = TierColors[(int)data.tier];
        }
    }

    private static Sprite CreatePlaceholderSprite()
    {
        // 32×32 white square — same size as our real tiles
        var tex = new Texture2D(32, 32);
        var pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32f);
    }
}
