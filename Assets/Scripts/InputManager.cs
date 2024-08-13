using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] public bool isEnable = true;
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private float raycastDistance = 100f;

    private Vector3 lastPosition;
    private Ray lastRay;
    private RaycastHit lastHit;

    private Camera SceneCamera 
    {
        get
        {
            if (!sceneCamera)
            {
                sceneCamera = Camera.main;
            }
            return sceneCamera;
        }
    } 

    public Vector3 GetMousePositionOnLayer()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = SceneCamera.nearClipPlane;
        var ray = SceneCamera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance, placementLayermask)) return lastPosition;
        lastPosition = hit.point;
        lastRay = ray;      // Store the last ray used for the raycast
        lastHit = hit;      // Store the last raycast hit result
        return lastPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (lastRay.direction == Vector3.zero) return; // Ensure the ray has been initialized
        // Draw the ray from the origin to the point of hit
        Gizmos.DrawLine(lastRay.origin, lastHit.point);
        Gizmos.DrawSphere(lastHit.point, 0.1f);
    } 
}