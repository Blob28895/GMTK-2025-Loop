using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader = default;

    private float _cursorDistanceFromCamera = 10f;
    private Camera _mainCamera;

    private void Awake()
    {
        // Get the main camera in the scene and store a reference to it.
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.LookEvent += SetCursorPosition;
    }

    private void OnDisable()
    {
        _inputReader.LookEvent -= SetCursorPosition;
    }

    public void SetCursorPosition(Vector2 screenPosition)
    {
        Vector3 mousePosition = screenPosition;
        mousePosition.z = _cursorDistanceFromCamera;
        transform.position = _mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
