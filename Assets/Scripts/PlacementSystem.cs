
using System.Linq;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isEnable = true;
    [SerializeField] private Color previewColor = Color.white;
    [SerializeField] private Color occupiedColor = Color.red;
    
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject mouseIndicatorPrefab;
    [SerializeField] private GameObject cellIndicatorPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private PlaceableObjectDatabaseSO placeableObjectDatabase;
    [Header("Debug")]
    [SerializeField] private PlaceableObject currentPlaceableObject; 
    
    //private
    private GridData gridData;
    private Renderer previewCellIndicatorRenderer;
    private GameObject mouseIndicator;
    private GameObject cellIndicator;

    private void Awake()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
        }
        
        InitializeIndicators();
    }

    private void Start()
    {
        gridData = new GridData();
        previewCellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        currentPlaceableObject = placeableObjectDatabase.placeableObjects.FirstOrDefault(x=>x.Id == 1);
    }

    private void Update()
    {
        if (inputManager.isEnable)
        {
            UpdateIndicators();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }
        else
        {
            DisableIndicators();
        }
    }

    #region Indicators

    private void InitializeIndicators()
    {
        if (mouseIndicatorPrefab != null)
        {
            mouseIndicator = Instantiate(mouseIndicatorPrefab);
            mouseIndicator.SetActive(false);
        }

        if (cellIndicatorPrefab != null)
        {
            cellIndicator = Instantiate(cellIndicatorPrefab);
            cellIndicator.SetActive(false);
        }
    }

    private void UpdateIndicators()
    {
        mouseIndicator.SetActive(true);
        cellIndicator.SetActive(true);

        Vector3 mousePosition = inputManager.GetMousePositionOnLayer();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        
        if(IsCellOccupied(gridPosition,currentPlaceableObject.Size))
        {
            previewCellIndicatorRenderer.material.color = occupiedColor;
        }
        else
        {
            previewCellIndicatorRenderer.material.color = previewColor;
        }
    }

    private void DisableIndicators()
    {
        if (mouseIndicator)
        {
            mouseIndicator.SetActive(false);
        }

        if (cellIndicator)
        {
            cellIndicator.SetActive(false);
        }
    }

    #endregion
   

    private void PlaceObject()
    {
        var mousePosition = inputManager.GetMousePositionOnLayer();
        var gridPosition = grid.WorldToCell(mousePosition);

        if (IsCellOccupied(gridPosition, currentPlaceableObject.Size))
        {
            Debug.LogWarning("Cell is already occupied");
            return;
        }

       
        if (currentPlaceableObject != null)
        {
            var newObject = InstantiatePlaceableObject(currentPlaceableObject, gridPosition);
            AddPlaceableDataToGridData(gridPosition, currentPlaceableObject);
        }
        else
        {
            Debug.LogError("No Placeable Object found in the database.");
        }
    }

    

    private bool IsCellOccupied(Vector3Int gridPosition, Vector2Int size)
    {
        return gridData.IsCellOccupied(gridPosition, size);
    }
     

    private GameObject InstantiatePlaceableObject(PlaceableObject placeableObject, Vector3Int gridPosition)
    {
        var newObject = Instantiate(placeableObject.Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        return newObject;
    }

    private void AddPlaceableDataToGridData(Vector3Int gridPosition, PlaceableObject placeableObject)
    {
        gridData.AddPlacementData(gridPosition, placeableObject);
    }
}
