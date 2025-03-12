using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CharacterInteractNPC : MonoBehaviour
{

    public Camera playerCamera;

    private float interactDistance = 2.0f;
    bool inConversation;
    // private Animator animator;

    // private PlayerMovement player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // animator = gameObject.GetComponent<Animator>();
        // player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player Interact E was pressed");
            Interact();
        }
    }

    void Interact()
    {
        Debug.Log("Interact was called");

        if (inConversation)
        {
            Debug.Log("Skipping Line");
            GameManager.Instance.SkipLine();
        }
        else
        {
            RaycastHit npc_interact_hit;
            Debug.Log("Reached else statement");
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out npc_interact_hit, interactDistance))
            {
                Debug.Log("Raycast hit: " + npc_interact_hit.collider.gameObject.name);
                if (npc_interact_hit.collider.gameObject.TryGetComponent(out NPC npc))
                {
                    Debug.Log("Hit something (NPC)");
                    if (npc_interact_hit.collider.gameObject.name.Contains("NPC"))
                    {
                        print("Hit NPC");
                        GameManager.Instance.StartDialogue(npc.dialogueAsset.dialogue, npc.StartPosition, npc.npcName, 2);
                    }
                }
            }

        }
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
    }

    private void OnEnable()
    {
        GameManager.OnDialogueStarted += JoinConversation;
        GameManager.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        GameManager.OnDialogueStarted -= JoinConversation;
        GameManager.OnDialogueEnded -= LeaveConversation;
    }
}


