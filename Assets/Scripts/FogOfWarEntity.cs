using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FogOfWarEntity : MonoBehaviour
{

    [SerializeField] private FogOfWarUnitGroup unitGroup;
    [field: SerializeField] public BoundsSource Source { get; set; }
    [field: SerializeField] public bool BlocksView { get; set; }
    [field: SerializeField] public bool FogVisibility { get; set; }
    public bool Visible { get; private set; }
    public UnityEvent<bool> OnVisibilityChange;

    public void SetVisibility(HashSet<Vector2Int> visibleCells)
    {
        var boundCells = FogOfWarGrid.GetCellsWithinBounds(GetBounds());
        bool visibility = boundCells.Intersect(visibleCells).Any();
        if (Visible != visibility)
        {
            Visible = visibility;
            OnVisibilityChange?.Invoke(visibility);
        }
    }

    public HashSet<Vector2Int> OccupiedCells()
    {
        HashSet<Vector2Int> resultCells = new HashSet<Vector2Int>();
        Vector2Int min = FogOfWarGrid.WorldToGridPosition(GetBounds().min);
        Vector2Int max = new Vector2Int(Mathf.CeilToInt(GetBounds().max.x - 0.5001f), Mathf.CeilToInt(GetBounds().max.z - 0.5001f));
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                resultCells.Add(cell);
            }
        }
        return resultCells;
    }
    private void Start()
    {
        if (FogVisibility)
        {
            OnVisibilityChange?.Invoke(Visible);
        }
    }

    private void OnEnable()
    {
        if (!unitGroup.FogOfWarEntities.Contains(this))
            unitGroup.FogOfWarEntities.Add(this);
    }
    private void OnDisable()
    {
        if (unitGroup.FogOfWarEntities.Contains(this))
            unitGroup.FogOfWarEntities.Remove(this);
    }

    private Collider _collider;
    private Renderer _renderer;
    private Bounds GetBounds()
    {
        Bounds bounds;
        switch (Source)
        {
            case BoundsSource.Collider:
                if (_collider == null)
                    _collider = GetComponent<Collider>();
                bounds = _collider.bounds;
                break;
            case BoundsSource.Renderer:
                if (_renderer == null)
                    _renderer = GetComponent<Renderer>();
                bounds = _renderer.bounds;
                break;
            default:
                bounds = new Bounds();
                break;
        }
        return bounds;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(GetBounds().center, GetBounds().size);
    }

    public enum BoundsSource
    {
        Collider,
        Renderer
    }
}
