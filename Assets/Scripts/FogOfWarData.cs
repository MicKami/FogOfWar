using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FogOfWar/FogOfWarData", fileName = "FogOfWarData")]
public class FogOfWarData : ScriptableObject
{
    public HashSet<Vector2Int> StaticFogCells { get; set; } = new();
    public HashSet<Vector2Int> DynamicFogCells { get; set; } = new();
    public HashSet<Vector2Int> BlockedCells { get; set; } = new();
    [field: SerializeField]
    public FogOfWarGrid Grid { get; set; }

    public void Clear()
    {
        StaticFogCells.Clear();
        DynamicFogCells.Clear();
        BlockedCells.Clear();
    }
}
