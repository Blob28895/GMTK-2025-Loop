using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lasso : MonoBehaviour
{
    // Rope prefab
    public GameObject ropeLine = default;
    [SerializeField] private InputReaderSO _inputReader = default;

    private Rope _currentRope = default;
    private Vector3 _currMousePosition = default;
    private float _cursorDistanceFromCamera = 10f;
    private Camera _mainCamera;

    private void Awake()
    {
        // Get the main camera in the scene and store a reference to it.
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.AttackEvent += DrawRope;
        _inputReader.LookEvent += SetCursorPosition;
    }

    private void OnDisable()
    {
        _inputReader.AttackEvent -= DrawRope;
        _inputReader.LookEvent -= SetCursorPosition;
    }

    public void DrawRope(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _currentRope = Instantiate(ropeLine, _currMousePosition, Quaternion.identity).GetComponent<Rope>();
        }

        if (context.phase == InputActionPhase.Canceled && _currentRope != null)
        {
            Destroy(_currentRope);
            GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");
            Array.ForEach(ropes, rope => { Destroy(rope); });
        }
    }

    public void SetCursorPosition(Vector2 screenPosition)
    {
        _currMousePosition = _mainCamera.ScreenToWorldPoint(screenPosition);
        _currMousePosition.z = _cursorDistanceFromCamera;
        transform.position = _currMousePosition;

        if(_currentRope != null)
        {
            _currentRope.SetPosition(_currMousePosition);
        }   
    }
}
