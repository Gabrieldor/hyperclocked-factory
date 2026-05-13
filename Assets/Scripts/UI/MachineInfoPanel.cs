using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Shows runtime info for a tapped machine: name, recipe selection, state, progress, remove.
public class MachineInfoPanel : MonoBehaviour
{
    public static MachineInfoPanel Instance { get; private set; }

    [SerializeField] private GameObject     panel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI stateLabel;
    [SerializeField] private Slider          progressBar;
    [SerializeField] private Transform       recipeContainer;
    [SerializeField] private GameObject      recipeButtonPrefab; // TMP_Text + Button
    [SerializeField] private Button          removeButton;
    [SerializeField] private Button          closeButton;

    private PlacedMachine _current;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        if (panel != null) panel.SetActive(false);
        if (removeButton != null) removeButton.onClick.AddListener(OnRemove);
        if (closeButton  != null) closeButton.onClick.AddListener(Close);
    }

    public void Open(PlacedMachine placed)
    {
        _current = placed;
        if (panel != null) panel.SetActive(true);
        Refresh();
        RebuildRecipeButtons();
    }

    public void Close()
    {
        _current = null;
        if (panel != null) panel.SetActive(false);
    }

    private void Update()
    {
        if (_current == null) return;
        if (panel == null || !panel.activeSelf) return;
        // Live-update state and progress every frame
        if (stateLabel  != null) stateLabel.text  = _current.instance?.State.ToString() ?? "-";
        if (progressBar != null) progressBar.value = _current.instance?.Progress ?? 0f;
    }

    private void Refresh()
    {
        if (nameLabel  != null) nameLabel.text  = _current.data.machineName;
        if (stateLabel != null) stateLabel.text = _current.instance?.State.ToString() ?? "-";
        if (progressBar != null) progressBar.value = _current.instance?.Progress ?? 0f;
    }

    private void RebuildRecipeButtons()
    {
        if (recipeContainer == null) return;
        foreach (Transform child in recipeContainer) Destroy(child.gameObject);
        if (_current.data.availableRecipes == null) return;

        foreach (var recipe in _current.data.availableRecipes)
        {
            if (recipeButtonPrefab == null) break;
            var btn = Instantiate(recipeButtonPrefab, recipeContainer);
            var label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = recipe.name;

            var r = recipe;
            btn.GetComponentInChildren<Button>()?.onClick.AddListener(() =>
            {
                _current.instance?.SetRecipe(r);
                Refresh();
            });
        }
    }

    private void OnRemove()
    {
        if (_current == null) return;
        var cell = _current.cell;
        var machineName = _current.data.machineName;
        Close();
        InventoryScreenUI.Instance?.ShowPickupConfirm(cell, machineName);
    }
}
