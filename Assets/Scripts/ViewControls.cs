using Unity.Cinemachine;
using UnityEngine;

public class ViewControls : MonoBehaviour
{
    public CinemachineInputAxisController camInputController;
    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            camInputController.Controllers[0].Enabled = true;
            camInputController.Controllers[1].Enabled = true;
        } else if (Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.None;
            camInputController.Controllers[0].Enabled = false;
            camInputController.Controllers[1].Enabled = false;
        }
    }
}