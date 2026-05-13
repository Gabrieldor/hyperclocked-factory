using System.Collections.Generic;
using UnityEngine;

/// Extractor: placed on a resource node, produces 1× node resource per tick.
/// No inputs, no recipe — just raw production.
public class ExtractorInstance : MachineInstance
{
    private NodeInstance _node;

    public void SetNode(NodeInstance node)
    {
        _node = node;
        if (_node != null)
        {
            _node.hasExtractor = true;
            _node.view?.Refresh();
        }
        State = (_node != null && _node.assignedResource != null)
            ? MachineState.Idle
            : MachineState.Halted;
    }

    public void ClearNode()
    {
        if (_node != null)
        {
            _node.hasExtractor = false;
            _node.view?.Refresh();
        }
        _node = null;
        State = MachineState.Halted;
    }

    protected override void OnTick()
    {
        if (_node == null || _node.assignedResource == null) { State = MachineState.Halted; return; }

        var item = _node.assignedResource;
        var (path, dest) = PipeNetwork.Instance != null
            ? PipeNetwork.Instance.FindPathToAcceptor(cell, item)
            : (null, default);

        if (path == null) { State = MachineState.OutputFull; return; }

        PipeNetwork.Instance.DispatchItem(item, path, dest);
        State = MachineState.Processing;
    }
}
