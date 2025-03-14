using Unity.VisualScripting;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    private bool isMoving;
    public float speed = 0.5f;
    private Transform goalPosition;
    private Animator animator;
    private bool inConversation = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoving = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
            turnTowardsPlayer(goalPosition);
            Transform playerPos = goalPosition.transform;
            // float step = speed * Time.deltaTime;
            Vector3 newYPos = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);   // Disregard the player's x and z position.


            transform.position = Vector3.MoveTowards(transform.position, newYPos, speed * Time.deltaTime);


        }
    }

    public void moveToSpot(Transform position)
    {
        turnTowardsPlayer(position);
        isMoving = true;
        goalPosition = position;
        Debug.Log("movetospot called!");
    }
    public void turnTowardsPlayer(Transform playerTransform)
    {
        Vector3 rot = Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles;
        rot.x = rot.z = 0;
        transform.rotation = Quaternion.Euler(rot);
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.name == "Player")
        {
            if (!inConversation)
            {
                inConversation = true;
                Debug.Log("HIT Player!");
                isMoving = false;
                animator.SetBool("isMoving", false);
                NPC npc = GetComponent<NPC>();
                print("Hit NPC");
                int p = npc.getPosition();
                GameManager.Instance.StartDialogue(npc.dialogueAsset.dialogue, p, npc.npcName, 2);
                npc.incPosition();
            }
        }
    }

    public void leaveConversation()
    {
        inConversation = false;
    }


}
