using UnityEngine;

public class ZoomTrigger : MonoBehaviour
{
    public CameraZoom cameraZoom;
    private bool isPlayerInside = false;
    private bool pressCameraZoom = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
            cameraZoom.isZoomedIn = false;
            pressCameraZoom = false; // Reset the pressCameraZoom flag
        }
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            ButtonPress();

            if (pressCameraZoom)
            {
                cameraZoom.isZoomedIn = true;
            }
            else
            {
                cameraZoom.isZoomedIn = false;
            }
        }
    }

    private void ButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressCameraZoom = !pressCameraZoom;
        }
    }
}
