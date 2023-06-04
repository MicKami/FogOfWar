using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FogOfWarVisionBase : MonoBehaviour
{
    [SerializeField] protected FogOfWarUnitGroup unitGroup;
    [field: SerializeField]
    public int VisibilityRadius { get; set; }
    public abstract HashSet<Vector2Int> VisibleCells(HashSet<Vector2Int> blockedCells);

    protected virtual void OnEnable()
    {
        if (!unitGroup.FogOfWarVisions.Contains(this))
            unitGroup.FogOfWarVisions.Add(this);
    }
    protected virtual void OnDisable()
    {
        if (unitGroup.FogOfWarVisions.Contains(this))
            unitGroup.FogOfWarVisions.Remove(this);
    }
}
