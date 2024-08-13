using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectDatabase", menuName = "Placeable/PlaceableObjectDatabase")]
public class PlaceableObjectDatabaseSO : ScriptableObject
{ 
    public List<PlaceableObject> placeableObjects;
    
    private void OnValidate()
    {
        UpdatePlaceableObjectIds();
    }

    private void UpdatePlaceableObjectIds()
    {
        for (var i = 0; i < placeableObjects.Count; i++)
        {
            placeableObjects[i].SetId(i);
        }
    }
}

[Serializable]
public class PlaceableObject
{
    [field: SerializeField]
    public int Id { get; private set; }
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public Sprite Icon { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; }

    public event Action OnChanged;

    public void SetId(int id)
    {
        Id = id;
        OnChanged?.Invoke();
    }

    public void SetName(string name)
    {
        Name = name;
        OnChanged?.Invoke();
    }

    public void SetPrefab(GameObject prefab)
    {
        Prefab = prefab;
        OnChanged?.Invoke();
    }

    public void SetIcon(Sprite icon)
    {
        Icon = icon;
        OnChanged?.Invoke();
    }
}