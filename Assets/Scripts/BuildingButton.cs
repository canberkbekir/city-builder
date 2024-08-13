using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
   [SerializeField] private PlaceableObject placeableObject;
   [SerializeField] private Image icon;

   private void OnEnable()
   {
      if (placeableObject != null)
      {
         placeableObject.OnChanged += UpdateIcon;
      }
   }

   private void OnDisable()
   {
      if (placeableObject != null)
      {
         placeableObject.OnChanged -= UpdateIcon;
      }
   }

   public void SetPlaceableObject(PlaceableObject obj)
   {
      if (placeableObject != null)
      {
         placeableObject.OnChanged -= UpdateIcon;
      }

      placeableObject = obj;
      icon.sprite = obj.Icon;

      if (placeableObject != null)
      {
         placeableObject.OnChanged += UpdateIcon;
      }
   }

   private void UpdateIcon()
   {
      icon.sprite = placeableObject.Icon;
   }
}
