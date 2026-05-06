using System.Collections;
using UnityEngine;

public class camZoomPoint : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private float zoomLevel;
    [SerializeField] private float zoomDuration;

    public static CameraFunction Instance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(SmoothZoom());
    }

    private IEnumerator SmoothZoom()
    {
        float size = Cam.orthographicSize;
        float time = 0f;

        while (time < zoomDuration)
        {
            time += Time.deltaTime;
            float t = time / zoomDuration;

            Cam.orthographicSize = Mathf.Lerp(size, zoomLevel, t);
            yield return null;
        }

        Cam.orthographicSize = zoomLevel;
    }
}