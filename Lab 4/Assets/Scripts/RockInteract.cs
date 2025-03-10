using UnityEngine;

public class RockInteract : MonoBehaviour
{
    public bool isPickedUp = false;
    private Rigidbody rb;
    private Transform originalParent;

    
    public float throwForce = 10f;
    private Vector3 throwDirection;

    
    public Vector3 heldPositionOffset = new Vector3(0, 0, 2); 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent; 
        rb.isKinematic = true; 
    }

    public void PickUp(Transform playerCamera)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            Debug.Log("Rock picked up!");

            // Rock follows camera
            transform.SetParent(playerCamera);
            rb.isKinematic = true; 
        }
    }

    public void UpdateHeldPosition(Vector3 cameraPosition, Quaternion cameraRotation)
    {
        if (isPickedUp)
        {
            transform.position = cameraPosition + cameraRotation * heldPositionOffset;
            transform.rotation = cameraRotation; 
        }
    }

    // Throw the rock
    public void Throw(Vector3 throwDirection)
    {
        if (isPickedUp)
        {
            isPickedUp = false;
            Debug.Log("Rock thrown!");

            transform.SetParent(originalParent);
            rb.isKinematic = false; 
            rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange); 

            rb.linearVelocity = Vector3.zero;
        }
    }
}
