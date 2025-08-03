using UnityEngine;
using UnityEngine.InputSystem;

public class Lasso : MonoBehaviour
{
    // Rope prefab
    public GameObject ropeLine = default;
    // CapturePoint prefab
    public GameObject capturePoint = default;

    [SerializeField] private InputReaderSO _inputReader = default;

    private Rope _currentRope = default;
    private Vector3 _currMouseWorldPosition = default;
    private Vector2 _mouseScreenPosition = default; // Stores the 2D screen position of the mouse
    private float _cursorDistanceFromCamera = 10f;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.AttackEvent += DrawRope;
        // Subscribe a new method to only store the latest mouse position
        _inputReader.LookEvent += OnLook;
    }

    private void OnDisable()
    {
        _inputReader.AttackEvent -= DrawRope;
        _inputReader.LookEvent -= OnLook;
    }

    private void LateUpdate()
    {
        // This logic now runs every frame, after the camera has moved.

        // The z-coordinate for ScreenToWorldPoint represents the distance from the camera into the scene.
        // We create a new Vector3 from our 2D screen position and this distance.
        Vector3 screenPointWithDepth = new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, _cursorDistanceFromCamera);
        _currMouseWorldPosition = _mainCamera.ScreenToWorldPoint(screenPointWithDepth);

        // Update the cursor's position every frame.
        transform.position = _currMouseWorldPosition;

        // If a rope is active, update its position as well.
        if (_currentRope != null)
        {
            _currentRope.SetPosition(_currMouseWorldPosition);
        }
    }

    /// <summary>
    /// This method is called by the LookEvent and simply updates the stored screen position.
    /// </summary>
    private void OnLook(Vector2 screenPosition)
    {
        _mouseScreenPosition = screenPosition;
    }

    public void DrawRope(InputAction.CallbackContext context)
    {
        // This method now uses the world position that is correctly updated every frame.
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Instantiating new rope");
            _currentRope = Instantiate(ropeLine, _currMouseWorldPosition, Quaternion.identity).GetComponent<Rope>();
            Instantiate(capturePoint, _currMouseWorldPosition, Quaternion.identity);
        }

        if (context.phase == InputActionPhase.Canceled && _currentRope != null)
        {
            // The original code had a potential issue where it would destroy the component but not the GameObject.
            // Destroying the GameObject is generally safer.
            Destroy(_currentRope.gameObject);

            // This search is expensive. If you only ever have one rope, a direct reference is better.
            GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");
            foreach (var rope in ropes)
            {
                Destroy(rope);
            }
        }
    }
}