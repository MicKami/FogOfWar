using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarMouseVision : FogOfWarVisionBase
{
    [SerializeField]
    private LayerMask layerMask;

    public override HashSet<Vector2Int> VisibleCells(HashSet<Vector2Int> blockedCells)
    {
        HashSet<Vector2Int> result = new();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
        {
            result.UnionWith(LineOfSight.TraceInRadius(FogOfWarGrid.WorldToGridPosition(hit.point), VisibilityRadius, blockedCells));
        }
        return result;
    }

    public void SetVisibilityRadius(float value)
    {
        VisibilityRadius = (int)value;
    }
}
