
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject mouseIndicatorPrefab;
    [SerializeField] private GameObject cellIndicatorPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private PlaceableObjectDatabaseSO placeableObjectDatabase;

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
        
        if(IsCellOccupied(gridPosition))
        {
            previewCellIndicatorRenderer.material.color = Color.red;
        }
        else
        {
            previewCellIndicatorRenderer.material.color = Color.white;
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

    private void PlaceObject()
    {
        Vector3 mousePosition = inputManager.GetMousePositionOnLayer();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (IsCellOccupied(gridPosition))
        {
            Debug.LogWarning("Cell is already occupied");
            return;
        }

        PlaceableObject placeableObject = GetPlaceableObject();
        if (placeableObject != null)
        {
            var newObject = InstantiatePlaceableObject(placeableObject, gridPosition);
            AddPlaceableDataToGridData(gridPosition, placeableObject);
        }
        else
        {
            Debug.LogError("No Placeable Object found in the database.");
        }
    }

    private PlaceableObject GetPlaceableObject()
    {
        // This method can be extended to choose different objects based on user input or game logic
        if (placeableObjectDatabase && placeableObjectDatabase.placeableObjects.Count > 0)
        {
            return placeableObjectDatabase.placeableObjects[0];
        }

        return null;
    }

    private bool IsCellOccupied(Vector3Int gridPosition)
    {
        return gridData.IsCellOccupied(gridPosition);
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
