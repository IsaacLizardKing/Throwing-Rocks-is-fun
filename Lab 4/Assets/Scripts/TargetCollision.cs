using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Target Hit: " + collision.gameObject.name);
    }

    // void onCollision
    // {
    //     Debug.Log("Target Hit: " + other.name);
    // }
}
