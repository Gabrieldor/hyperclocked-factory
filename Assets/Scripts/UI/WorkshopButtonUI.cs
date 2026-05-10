using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WorkshopButtonUI : MonoBehaviour
{
    [SerializeField] private MachineData workshopData;

    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable()
    {
        GridManager.OnMachinePlaced += OnGridChanged;
        GridManager.OnMachineRemoved += OnGridChanged;
        Refresh();
    }

    private void OnDisable()
    {
        GridManager.OnMachinePlaced -= OnGridChanged;
        GridManager.OnMachineRemoved -= OnGridChanged;
    }

    private void OnGridChanged(PlacedMachine _) => Refresh();

    private void Refresh()
    {
        _button.interactable = workshopData != null && GridManager.Instance != null
            && GridManager.Instance.IsWorkshopPlaced(workshopData);
    }
}
