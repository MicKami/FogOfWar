using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarUnitVision : FogOfWarVisionBase
{
    public override HashSet<Vector2Int> VisibleCells(HashSet<Vector2Int> blockedCells)
    {
        return LineOfSight.TraceInRadius(FogOfWarGrid.WorldToGridPosition(transform.position), VisibilityRadius, blockedCells);
    }
}
