using UnityEngine;

public class MagicIndicator : MonoBehaviour
{
    public LayerMask groundLayer; // LayerMask to specify which layers are considered as ground
    public float heightAboveGround = 0.1f; // Height above the ground
    public float heightAboveLimit = 3.0f; // Height above the plane to check for obstacles

    private void Update() {
        Vector3 planePosition = transform.position;

        // Cast a ray downwards from the plane's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer)) {
            // Position the plane at a fixed height above the ground
            if (hit.point.y < transform.position.y) {
                planePosition.y = hit.point.y + heightAboveGround;
            }

            // Optional: Adjust the plane's rotation to match the ground's normal
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        // Cast a ray upwards from the plane's position to detect obstacles above
        if (Physics.Raycast(transform.position, Vector3.up, out hit, heightAboveLimit, groundLayer)) {
            // Adjust the plane's position if an obstacle is detected above within the height limit
            if (hit.point.y > transform.position.y) {
                planePosition.y = Mathf.Min(planePosition.y, hit.point.y - heightAboveGround);
            }
        }

        transform.position = planePosition;
    }
}
