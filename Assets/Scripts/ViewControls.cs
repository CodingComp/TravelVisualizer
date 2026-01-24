using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewControls : MonoBehaviour
{
    public CinemachineOrbitalFollow camOrbit;
    public CinemachineInputAxisController camInputController;
    public Camera mainCamera;

    private const int EarthLayer = 7;
    private int _earthLayerMask;

    public float scrollMult = 2.0f;
    public float radiusMax = 10.0f;
    public float radiusMin = 3.0f;
    
    private void Awake()
    {
        _earthLayerMask = 1 << EarthLayer;
    }

    public void Update()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0) {
            mouseScroll *= -1*scrollMult;
            if (mouseScroll + camOrbit.Radius >= radiusMin && mouseScroll + camOrbit.Radius <= radiusMax) camOrbit.Radius += mouseScroll;
        }
        
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.None;
            camInputController.Controllers[0].Enabled = false;
            camInputController.Controllers[1].Enabled = false;
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool hitEarth = Physics.Raycast(ray, out RaycastHit _, Mathf.Infinity, _earthLayerMask);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (!isOverUI && hitEarth && Mouse.current.leftButton.wasPressedThisFrame) {
            Cursor.lockState = CursorLockMode.Locked;
            camInputController.Controllers[0].Enabled = true;
            camInputController.Controllers[1].Enabled = true;
        }
    }
}