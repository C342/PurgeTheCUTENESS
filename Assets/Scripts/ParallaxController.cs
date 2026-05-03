using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Camera mainCamera;

    [Range(0f, 1f)]
    public float parallaxFactorX = 0.3f;

    [Range(0f, 1f)]
    public float parallaxFactorY = 0.15f;

    private Vector3 lastCameraPos;

    void Start()
    {
        lastCameraPos = mainCamera.transform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = mainCamera.transform.position - lastCameraPos;

        transform.position += new Vector3(
            delta.x * parallaxFactorX,
            delta.y * parallaxFactorY,
            0f
        );

        lastCameraPos = mainCamera.transform.position;
    }
}