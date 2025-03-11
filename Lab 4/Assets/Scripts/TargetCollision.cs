using UnityEngine;

public class TargetCollision : MonoBehaviour
{

    [SerializeField] private ParticleSystem particles;

    private ParticleSystem particlesInstance;
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
        spawnParticles();
    }

    private void spawnParticles()
    {
        particlesInstance = Instantiate(particles, transform.position, Quaternion.LookRotation(transform.up, transform.up));
    }

}
