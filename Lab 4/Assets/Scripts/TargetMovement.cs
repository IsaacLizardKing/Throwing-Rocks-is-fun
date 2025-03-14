using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        targetMove();
    }

    void targetMove()
    {
        // float step = speed * Time.deltaTime;
        Vector3 newYPos = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);   // Disregard the player's x and z position.


        transform.position = Vector3.MoveTowards(transform.position, newYPos, speed * Time.deltaTime);
        if (transform.position.x > 20)
        {
            transform.position = new Vector3(-20, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -20)
        {
            transform.position = new Vector3(20, transform.position.y, transform.position.z);

        }
    }
}
