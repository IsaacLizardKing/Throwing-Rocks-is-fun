using UnityEngine;

public class TargetCollision : MonoBehaviour
{

    [SerializeField] private ParticleSystem particles;
    public GameObject NPC;
    private NPCMove npcMove;
    public GameObject player;

    private ParticleSystem particlesInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcMove = NPC.GetComponent<NPCMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Target Hit: " + collision.gameObject.name);
        spawnParticles();
        GameManager.Instance.targetHit(gameObject);
        npcMove.moveToSpot(player.transform);
        GameManager.Instance.IncScore(1);
    }

    private void spawnParticles()
    {
        particlesInstance = Instantiate(particles, transform.position, Quaternion.LookRotation(transform.up, transform.up));
    }

}
