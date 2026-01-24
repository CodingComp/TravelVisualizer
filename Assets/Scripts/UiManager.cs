using System;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public RectTransform mainUiContainer;
    public GameObject tabIcon;

    [SerializeField] private float inactivityTime = 10f;
    private float _lastInputTime;

    private float _uiTargetAlpha = 0.0f;
    private float _currentAlpha;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float posLerpSpeed = 6f; 
    
    public CanvasGroup mainUiGroup;
    public CanvasGroup controlsGroup;

    private bool _mainUiOpen;
    private float _mainUiClosePos = 139.0f;
    private float _mainUiCurrentPos;
    private float _mainUiTargetPos;

    private float _currentTabRot;
    private float _tabTargetRot = 180.0f;

    private void Awake()
    {
        _mainUiClosePos = _mainUiCurrentPos = _mainUiTargetPos = mainUiContainer.position.y;
        _currentTabRot = tabIcon.transform.localRotation.eulerAngles.z;
        mainUiGroup.alpha = 0.0f;
        controlsGroup.alpha = 0.0f;
    }

    void Start()
    {
        _lastInputTime = Time.time;
    }

    void Update()
    {
        // Check if any input is active
        if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || 
            Input.touchCount > 0 || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f || 
            Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f) {
            _lastInputTime = Time.time;
            _uiTargetAlpha = 1.0f;
        }

        // Check if enough time has passed since last input
        if (Time.time - _lastInputTime >= inactivityTime) {
            _uiTargetAlpha = 0.0f;
            _lastInputTime = Time.time;
            if (_mainUiOpen) ToggleMainUi();
        }
        
        // Lerp to target pos
        
        _currentAlpha = Mathf.Lerp(_currentAlpha, _uiTargetAlpha, lerpSpeed * Time.deltaTime);
        mainUiGroup.alpha = _currentAlpha;
        controlsGroup.alpha = _currentAlpha;
        
        _mainUiCurrentPos = Mathf.Lerp(_mainUiCurrentPos, _mainUiTargetPos, posLerpSpeed * Time.deltaTime);
        mainUiContainer.anchoredPosition = new Vector3(0.0f, _mainUiCurrentPos, 0.0f);
        
        _currentTabRot = Mathf.Lerp(_currentTabRot, _tabTargetRot, posLerpSpeed * Time.deltaTime);
        tabIcon.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _currentTabRot);
    }

    public void ToggleMainUi()
    {
        _mainUiOpen = !_mainUiOpen;
        _mainUiTargetPos = _mainUiOpen ? 0.0f : _mainUiClosePos;
        _tabTargetRot = _mainUiOpen ? 0.0f : 180.0f;
        
        print(_mainUiTargetPos + " " + _tabTargetRot);
    }
}
