using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FogOfWarGrid
{
    [field: SerializeField]
    public Vector2 Size { get; private set; } = new Vector2(32, 32);

    [field: SerializeField]
    public float CellSize { get; private set; } = 1;

    public Vector2Int CellCount => new Vector2Int(Mathf.FloorToInt(Size.x / CellSize), Mathf.FloorToInt(Size.y / CellSize));
    public Vector2Int SizeInt => new Vector2Int(Mathf.FloorToInt(Size.x), Mathf.FloorToInt(Size.y));
    public Vector3 Center => new Vector3(((CellCount.x % 2) - 1) / 2f * CellSize, 0, ((CellCount.y % 2) - 1) / 2f * CellSize);

    public static Vector2Int WorldToGridPosition(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x + 0.5001f), Mathf.FloorToInt(position.z + 0.5001f));
    }

    public static Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y);
    }

    public static HashSet<Vector2Int> GetCellsWithinBounds(Bounds bounds)
    {
        HashSet<Vector2Int> resultCells = new HashSet<Vector2Int>();
        Vector2Int min = WorldToGridPosition(bounds.min);
        Vector2Int max = new Vector2Int(Mathf.CeilToInt(bounds.max.x - 0.5001f), Mathf.CeilToInt(bounds.max.z - 0.5001f));
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
}
