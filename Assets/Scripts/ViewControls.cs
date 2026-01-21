using Unity.Cinemachine;
using UnityEngine;

public class ViewControls : MonoBehaviour
{
    public CinemachineInputAxisController camInputController;
    public Camera mainCamera;

    private const int EarthLayer = 7;
    private int _earthLayerMask;

    private void Awake()
    {
        _earthLayerMask = 1 << EarthLayer;
    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.None;
            camInputController.Controllers[0].Enabled = false;
            camInputController.Controllers[1].Enabled = false;
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool hitEarth = Physics.Raycast(ray, out RaycastHit _, Mathf.Infinity, _earthLayerMask);

        if (hitEarth && Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            camInputController.Controllers[0].Enabled = true;
            camInputController.Controllers[1].Enabled = true;
        }
    }
}