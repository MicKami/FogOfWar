using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField]
    private FogOfWarData data;
    [SerializeField]
    private FogOfWarUnitGroup unitGroup;


    private void Awake()
    {
        data.Clear();
    }

    private void Update()
    {
        data.DynamicFogCells.Clear();
        data.BlockedCells.Clear();

        HashSet<Vector2Int> potentiallyVisibleCells = new();
        foreach (var fogEntity in unitGroup.FogOfWarEntities)
        {
            if (fogEntity.BlocksView)
            {
                var boundCells = fogEntity.OccupiedCells();
                if (fogEntity.FogVisibility)
                {
                    potentiallyVisibleCells.UnionWith(boundCells);
                }
                data.BlockedCells.UnionWith(boundCells);
            }
        }

        foreach (var vision in unitGroup.FogOfWarVisions)
        {
            var visibleCells = vision.VisibleCells(data.BlockedCells);
            data.DynamicFogCells.UnionWith(visibleCells);
            data.StaticFogCells.UnionWith(visibleCells);
        }

        List<Vector2Int> expansionCells = new();
        foreach (var cell in potentiallyVisibleCells)
        {
            if (HasVisibleCellsAround(cell))
            {
                expansionCells.Add(cell);
            }
        }
        data.DynamicFogCells.UnionWith(expansionCells);
        data.StaticFogCells.UnionWith(expansionCells);

        foreach (var fogEntity in unitGroup.FogOfWarEntities)
        {
            if (fogEntity.FogVisibility)
            {
                fogEntity.SetVisibility(data.DynamicFogCells);
            }
        }
    }
    private bool HasVisibleCellsAround(Vector2Int cell)
    {
        return data.DynamicFogCells.Contains(cell + Vector2Int.up) || data.DynamicFogCells.Contains(cell + Vector2Int.down) || data.DynamicFogCells.Contains(cell + Vector2Int.left) || data.DynamicFogCells.Contains(cell + Vector2Int.right);
    }
    public void ResetVisibility()
    {
        data.DynamicFogCells.Clear();
        data.StaticFogCells.Clear();
    }
}
