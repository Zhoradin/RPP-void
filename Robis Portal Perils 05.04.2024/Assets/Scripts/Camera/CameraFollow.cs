using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string targetTag = "Player";
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 minValue, maxValue;

    private Transform target;

    private void Start()
    {
        // Find the object with the specified tag and assign its transform to the target variable
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("No object with tag '" + targetTag + "' found!");
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Follow();
        }
    }

    public void Follow()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
