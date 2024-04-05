using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSize = 5f;
    public float zoomSpeed = 5f;
    private float originalSize;
    public bool isZoomedIn = false;

    private void Start()
    {
        originalSize = GetComponent<Camera>().orthographicSize;
    }

    private void Update()
    {
        float targetSize = isZoomedIn ? zoomSize : originalSize;
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
}