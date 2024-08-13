using UnityEngine;

public class PlacementUI : MonoBehaviour
{
   [SerializeField] private PlaceableObjectDatabaseSO placeableObjectDatabase;
   [SerializeField] private GameObject placeableObjectButtonPrefab;

   private void Awake()
   {
         foreach (var placeableObject in placeableObjectDatabase.placeableObjects)
         {
            var newButton = Instantiate(placeableObjectButtonPrefab, transform);
            var buildingButton = newButton.GetComponent<BuildingButton>();
            buildingButton.SetPlaceableObject(placeableObject);
         }
   }
}
