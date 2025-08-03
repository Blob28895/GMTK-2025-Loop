using UnityEngine;

/// <summary>
/// Attach this script to your main Orthographic Camera.
/// It automatically adjusts the camera's orthographic size to ensure a consistent
/// field of view, regardless of the screen's aspect ratio.
/// This prevents the "zoom" level from changing on different monitors.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ResolutionIndependentCamera : MonoBehaviour
{
    [Tooltip("The target width of your game screen in world units. The camera will adjust its size to always show this width.")]
    [SerializeField] private float targetWidth = 18.0f;

    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        AdjustCameraSize();
    }

    /// <summary>
    /// Calculates and sets the orthographic size based on the screen aspect ratio.
    /// </summary>
    private void AdjustCameraSize()
    {
        // Ensure we have a camera and it's orthographic
        if (_camera == null || !_camera.orthographic)
        {
            return;
        }

        // Calculate the screen's current aspect ratio
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;

        // Calculate the required orthographic size to fit the target width
        float requiredOrthographicSize = targetWidth / screenAspectRatio / 2.0f;

        // Set the camera's size
        _camera.orthographicSize = requiredOrthographicSize;
    }

    // Optional: You can call this in Update if you allow runtime resolution changes,
    // but Awake() is usually sufficient for most games.
    // void Update()
    // {
    //     AdjustCameraSize();
    // }
}
