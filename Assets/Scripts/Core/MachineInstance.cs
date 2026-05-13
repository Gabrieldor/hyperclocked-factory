using System.Collections.Generic;
using UnityEngine;

/// Runtime MonoBehaviour attached to every placed machine GameObject.
/// Handles recipe processing, input/output buffers, and tick-driven state transitions.
public class MachineInstance : MonoBehaviour
{
    // Set by GridManager immediately after AddComponent
    public MachineData data;
    public Vector2Int cell;

    public static event System.Action<ItemData, MachineData> OnItemProduced;

    public MachineState State { get; protected set; } = MachineState.Idle;
    public RecipeData ActiveRecipe { get; private set; }
    public float Progress => _totalTicks > 0 ? (float)_elapsedTicks / _totalTicks : 0f;

    // input buffer: how many of each item we're holding
    protected readonly Dictionary<ItemData, int> InputBuffer = new();
    // output items waiting to leave
    protected readonly Queue<ItemData> OutputBuffer = new();

    private int _elapsedTicks;
    private int _totalTicks;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected virtual void OnEnable()
    {
        TickManager.OnTick        += OnTick;
        PipeNetwork.OnItemArrived += HandleItemArrived;
    }

    protected virtual void OnDisable()
    {
        TickManager.OnTick        -= OnTick;
        PipeNetwork.OnItemArrived -= HandleItemArrived;
    }

    public void Init(MachineData machineData, Vector2Int gridCell)
    {
        data = machineData;
        cell = gridCell;
        if (data.availableRecipes != null && data.availableRecipes.Length > 0)
            SetRecipe(data.availableRecipes[0]);
    }

    // ── Recipe ────────────────────────────────────────────────────────────────

    public void SetRecipe(RecipeData recipe)
    {
        if (recipe == ActiveRecipe) return;
        ActiveRecipe = recipe;
        InputBuffer.Clear();
        OutputBuffer.Clear();
        _elapsedTicks = 0;
        _totalTicks = recipe != null ? Mathf.RoundToInt(recipe.processingTime) : 0;
        State = MachineState.Idle;
    }

    // ── Item acceptance ───────────────────────────────────────────────────────

    public bool CanAcceptItem(ItemData item)
    {
        if (ActiveRecipe == null) return false;
        foreach (var stack in ActiveRecipe.inputs)
        {
            if (stack.item != item) continue;
            InputBuffer.TryGetValue(item, out int have);
            return have < stack.quantity;
        }
        return false;
    }

    private void HandleItemArrived(Vector2Int dest, ItemData item)
    {
        if (dest != cell) return;
        if (!CanAcceptItem(item)) return;
        InputBuffer.TryGetValue(item, out int current);
        InputBuffer[item] = current + 1;
    }

    // ── Tick ─────────────────────────────────────────────────────────────────

    protected virtual void OnTick()
    {
        switch (State)
        {
            case MachineState.Idle:
            case MachineState.NoInput:
                if (HasAllInputs()) StartProcessing();
                else State = MachineState.NoInput;
                break;

            case MachineState.Processing:
                _elapsedTicks++;
                if (_elapsedTicks >= _totalTicks) FinishProcessing();
                break;

            case MachineState.OutputFull:
                TryDispatchOutputs();
                break;
        }
    }

    private bool HasAllInputs()
    {
        if (ActiveRecipe == null) return false;
        foreach (var stack in ActiveRecipe.inputs)
        {
            InputBuffer.TryGetValue(stack.item, out int have);
            if (have < stack.quantity) return false;
        }
        return true;
    }

    private void StartProcessing()
    {
        foreach (var stack in ActiveRecipe.inputs)
        {
            InputBuffer[stack.item] -= stack.quantity;
            if (InputBuffer[stack.item] <= 0) InputBuffer.Remove(stack.item);
        }
        _elapsedTicks = 0;
        State = MachineState.Processing;
    }

    private void FinishProcessing()
    {
        foreach (var stack in ActiveRecipe.outputs)
        {
            for (int i = 0; i < stack.quantity; i++)
                OutputBuffer.Enqueue(stack.item);
            OnItemProduced?.Invoke(stack.item, data);
        }

        State = MachineState.OutputFull;
        TryDispatchOutputs();
    }

    protected virtual bool TryDispatchOutputs()
    {
        while (OutputBuffer.Count > 0)
        {
            var item = OutputBuffer.Peek();
            var (path, dest) = PipeNetwork.Instance != null
                ? PipeNetwork.Instance.FindPathToAcceptor(cell, item)
                : (null, default);

            if (path == null) { State = MachineState.OutputFull; return false; }

            OutputBuffer.Dequeue();
            PipeNetwork.Instance.DispatchItem(item, path, dest);
        }

        State = HasAllInputs() ? MachineState.Processing : MachineState.Idle;
        if (State == MachineState.Processing) StartProcessing();
        return true;
    }
}
