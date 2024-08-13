using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> data = new();

    public void AddPlacementData(Vector3Int gridPosition, PlaceableObject obj)
    {
        try
        {
            var positionToOccupy = CalculatePosition(gridPosition, obj.Size);
            var placementData = new PlacementData(positionToOccupy, obj);

            if (positionToOccupy.Any(pos => data.ContainsKey(pos)))
            {
                Debug.LogWarning("Cell is already occupied");
            }

            foreach (var pos in positionToOccupy)
            {
                data[pos] = placementData;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void RemovePlacementData(Vector3Int position)
    {
        var currentData = data[position];
        if (currentData == null) return;
        
        foreach (var pos in currentData.occupiedCells)
        {
            data.Remove(pos);
        }

    }

    public bool IsCellOccupied(Vector3Int position)
    {
        return data.ContainsKey(position);
    }
    
    public bool IsCellOccupied(Vector3Int position,Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                if (data.ContainsKey(position + new Vector3Int(x, 0, y)))
                {
                    return true;
                }
            }
        }
        return false; 
    }

    public PlacementData GetPlacementData(Vector3Int position)
    {
        data.TryGetValue(position, out var placementData);
        return placementData;
    }

    public void ClearAllData()
    {
        data.Clear();
    }

    public List<Vector3Int> GetAllOccupiedCells()
    {
        return data.Keys.ToList();
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int size)
    {
        var result = new List<Vector3Int>();
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                result.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return result;
    }
}

public class PlacementData
{
    public readonly List<Vector3Int> occupiedCells;
    public PlaceableObject placeableObject { get; private set; }

    public PlacementData(List<Vector3Int> occupiedCells, PlaceableObject obj)
    {
        this.placeableObject = obj;
        this.occupiedCells = occupiedCells;
    }
}