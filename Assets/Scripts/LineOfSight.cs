using System.Collections.Generic;
using UnityEngine;

public static class LineOfSight
{
    public static HashSet<Vector2Int> CircleOutline(Vector2Int startPoint, int radius)
    {
        HashSet<Vector2Int> points = new();

        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            points.Add(new Vector2Int(x + startPoint.x, y + startPoint.y));
            points.Add(new Vector2Int(y + startPoint.x, x + startPoint.y));
            points.Add(new Vector2Int(-x + startPoint.x, y + startPoint.y));
            points.Add(new Vector2Int(-y + startPoint.x, x + startPoint.y));
            points.Add(new Vector2Int(-x + startPoint.x, -y + startPoint.y));
            points.Add(new Vector2Int(-y + startPoint.x, -x + startPoint.y));
            points.Add(new Vector2Int(x + startPoint.x, -y + startPoint.y));
            points.Add(new Vector2Int(y + startPoint.x, -x + startPoint.y));


            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                points.Add(new Vector2Int(x + startPoint.x - 1, y + startPoint.y - 1));
                points.Add(new Vector2Int(y + startPoint.x - 1, x + startPoint.y - 1));
                points.Add(new Vector2Int(-x + startPoint.x + 1, y + startPoint.y - 1));
                points.Add(new Vector2Int(-y + startPoint.x + 1, x + startPoint.y - 1));
                points.Add(new Vector2Int(-x + startPoint.x + 1, -y + startPoint.y + 1));
                points.Add(new Vector2Int(-y + startPoint.x + 1, -x + startPoint.y + 1));
                points.Add(new Vector2Int(x + startPoint.x - 1, -y + startPoint.y + 1));
                points.Add(new Vector2Int(y + startPoint.x - 1, -x + startPoint.y + 1));

                x--;
                radiusError += 2 * (y - x) + 1;
            }
        }

        return points;
    }

    public static HashSet<Vector2Int> Trace(Vector2Int start, Vector2Int end, HashSet<Vector2Int> blockedCells)
    {
        HashSet<Vector2Int> tracedCells = new();

        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = (start.x < end.x) ? 1 : -1;
        int sy = (start.y < end.y) ? 1 : -1;
        int error = dx - dy;

        Vector2Int current = start;

        while (true)
        {
            if (blockedCells.Contains(current))
            {
                break;
            }
            tracedCells.Add(current);
            if (current == end)
            {
                break;
            }
            int error2 = 2 * error;
            if (error2 > -dy && error2 < dx)
            {
                var left = new Vector2Int(current.x + sx, current.y);
                var right = new Vector2Int(current.x, current.y + sy);
                error -= dy;
                current.x += sx;
                error += dx;
                current.y += sy;
                if (blockedCells.Contains(left) || blockedCells.Contains(right))
                {
                    break;
                }
            }
            else if (error2 < dx)
            {
                error += dx;
                current.y += sy;
            }
            else if (error2 > -dy)
            {
                error -= dy;
                current.x += sx;
            }
        }
        return tracedCells;
    }

    public static HashSet<Vector2Int> TraceInRadius(Vector2Int start, int radius, HashSet<Vector2Int> blockedCells)
    {
        var result = new HashSet<Vector2Int>();
        var outlineCells = CircleOutline(start, radius);

        foreach (var cell in outlineCells)
        {
            result.UnionWith(Trace(start, cell, blockedCells));
        }
        return result;
    }
}
