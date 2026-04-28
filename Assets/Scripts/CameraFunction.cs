using System.Collections;
using UnityEngine;

public class CameraFunction : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f;
    //[SerializeField] private float shakeSpeed = 1f;
    //[SerializeField] private float magnitude = 0.05f;

    public static CameraFunction Instance;
    private Vector3 offset;
    private float seed;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        Vector3 targetPos = PlayerController.Instance.transform.position + offset;
        Vector3 followPos = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime * 60f);

        //float x = (Mathf.PerlinNoise(seed, Time.time * shakeSpeed) - 0.5f) * 2f;
        //float y = (Mathf.PerlinNoise(seed + 1, Time.time * shakeSpeed) - 0.5f) * 2f;

        //Vector3 shakeOffset = new Vector3(x, y, 0) * magnitude;

        transform.position = followPos;// + shakeOffset;
    }
}