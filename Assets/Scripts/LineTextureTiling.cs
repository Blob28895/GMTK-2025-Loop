using UnityEngine;

// Ensures this script is on a GameObject with a LineRenderer.
[RequireComponent(typeof(LineRenderer))]
public class LineTextureTiling : MonoBehaviour
{
    private LineRenderer line;

    void Awake()
    {
        // Get the LineRenderer component attached to this GameObject.
        line = GetComponent<LineRenderer>();

        // Ensure the texture mode is set to Tile.
        // This is an alternative to setting it on the texture asset directly.
        if (line.material.mainTexture != null)
        {
            line.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }
    }

    void Update()
    {
        // Get the total length of the line.
        float length = GetLineLength();

        // Adjust the material's texture scale.
        // The 'x' value controls the tiling along the line's length.
        // We keep 'y' at 1 to avoid vertical stretching.
        line.material.mainTextureScale = new Vector2(length, 1f);
    }

    private float GetLineLength()
    {
        float totalLength = 0f;

        // Iterate through all points except the last one.
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            // Find the distance between the current point and the next one.
            totalLength += Vector3.Distance(line.GetPosition(i), line.GetPosition(i + 1));
        }

        return totalLength;
    }
}