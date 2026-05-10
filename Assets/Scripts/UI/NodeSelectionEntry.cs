using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeSelectionEntry : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button button;

    public void Setup(ItemData resource, Sprite sprite, System.Action<ItemData> onSelected)
    {
        if (sprite != null) icon.sprite = sprite;
        label.text = resource.itemName;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelected(resource));
    }
}
