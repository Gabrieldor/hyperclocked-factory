using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual representation of a single inventory or hotbar slot.
/// Contains an icon image, a quantity label, and a highlight border.
///
/// Setup: call Setup(index, callback) once after instantiation (done by HotbarUI / InventoryScreenUI).
/// Refresh: call Refresh(slot, selected) whenever data changes.
///
/// Inspector assignments are made by InventoryUISetupEditor — no manual drag needed.
/// </summary>
[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image    iconImage;
    [SerializeField] private TMP_Text countLabel;
    [SerializeField] private Image    highlight;
    [SerializeField] [Range(0f, 1f)] private float highlightAlpha = 0.25f;

    private int                  _index;
    private System.Action<int>   _onTap;

    // Called once by HotbarUI or InventoryScreenUI to wire the button.
    public void Setup(int index, System.Action<int> onTap)
    {
        _index = index;
        _onTap = onTap;
        GetComponent<Button>().onClick.AddListener(() => _onTap(_index));
    }

    /// Refreshes the visual state from slot data.
    public void Refresh(InventorySlot slot, bool selected)
    {
        bool hasItem = slot != null && !slot.IsEmpty;

        if (iconImage != null)
        {
            iconImage.enabled = hasItem && slot.item.icon != null;
            if (hasItem && slot.item.icon != null)
                iconImage.sprite = slot.item.icon;
        }

        if (countLabel != null)
            countLabel.text = (hasItem && slot.quantity > 1) ? slot.quantity.ToString() : "";

        if (highlight != null)
        {
            highlight.enabled = selected;
            if (selected)
            {
                var c = highlight.color;
                c.a = highlightAlpha;
                highlight.color = c;
            }
        }
    }
}
