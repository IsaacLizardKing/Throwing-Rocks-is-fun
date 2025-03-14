using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance { get; private set; }


    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject dialoguePanel;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    public GameObject npc;
    public GameObject player;
    bool skipLineTriggered;

    public void StartDialogue(string[] dialogue, int startPosition, string name, int stopPosition)
    {
        Debug.Log("GameManager start dialog");
        Debug.Log(name);
        Debug.Log(dialogue);
        Debug.Log(startPosition);
        Debug.Log(nameText);
        nameText.text = name + "...";

        dialoguePanel.SetActive(true);

        StopAllCoroutines();

        StartCoroutine(RunDialogue(dialogue, startPosition, stopPosition));

    }

    IEnumerator RunDialogue(string[] dialogue, int position, int stopPosition)
    {
        Debug.Log("Game Manager run dialog");
        skipLineTriggered = false;
        OnDialogueStarted?.Invoke();

        // for (int i = startPosition; i < dialogue.Length; i++)
        // {
        //     Debug.Log("dialog i: " + i);
        //     Debug.Log("dialog length: " + dialogue.Length);
        //     if (i == stopPosition)
        //     {
        //         Debug.Log("Stop Position Reached");
        //         break;
        //     }
        //     //dialogueText.text = dialogue[i];
        //     dialogueText.text = null;
        //     StartCoroutine(TypeTextUncapped(dialogue[i]));

        //     while (skipLineTriggered == false)
        //     {
        //         // Wait for the current line to be skipped
        //         yield return null;
        //     }
        //     skipLineTriggered = false;
        // }
        dialogueText.text = dialogue[position];
        dialogueText.text = null;
        StartCoroutine(TypeTextUncapped(dialogue[position]));

        while (skipLineTriggered == false)
        {
            // Wait for the current line to be skipped
            yield return null;
        }
        skipLineTriggered = false;

        NPCMove npcMove = npc.GetComponent<NPCMove>();
        npcMove.leaveConversation();
        Debug.Log("Dialog ended");
        OnDialogueEnded?.Invoke();
        dialoguePanel.SetActive(false);
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void ShowDialogue(string dialogue, string name)
    {
        nameText.text = name + "...";
        StartCoroutine(TypeTextUncapped(dialogue));
        dialoguePanel.SetActive(true);
    }

    public void EndDialogue()
    {
        NPCMove npcMove = npc.GetComponent<NPCMove>();
        npcMove.leaveConversation();
        nameText.text = null;
        dialogueText.text = null;
        dialoguePanel.SetActive(false);
    }

    float charactersPerSecond = 90;

    IEnumerator TypeTextUncapped(string line)
    {
        Debug.Log("Game Manager Type Text Uncapped");
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }
    public void moveNPCtoSpot()
    {
        Debug.Log("GameManager move to spot");
        npc.GetComponent<NPCMove>().moveToSpot(player.transform);
    }
    public GameObject target;
    public void targetHit(GameObject target)
    {
        moveNPCtoSpot();
        Destroy(target);
        spawnTarget();
    }
    public void spawnTarget()
    {
        // pick a random point
        Vector3 p1 = new Vector3(15, 12, -15);
        Vector3 p2 = new Vector3(15, 8, -20);
        Vector3 p3 = new Vector3(15, 6, -20);
        Vector3 p4 = new Vector3(15, 12, -15);
        Vector3 p5 = new Vector3(15, 1, -12);

        List<Vector3> lst = new List<Vector3>
        {
            p1,
            p2,
            p3,
            p4
        };
        int i = UnityEngine.Random.Range(0, 3);
        // spawn target
        Instantiate(target, lst[i], Quaternion.identity);
    }

    // public void eatCarrot(GameObject player, Animator animator, GameObject carrot)
    // {
    //     animator.SetBool("IsEating", true);
    //     player.GetComponent<PlayerMovement>().disableMovement();
    //     ParticleSystem p = carrot.GetComponent<CarrotScript>().particleSystem;
    //     p.Play();
    //     StartCoroutine(eatCarrotRoutine(player, animator, carrot));
    // }

    // IEnumerator eatCarrotRoutine(GameObject player, Animator animator, GameObject carrot)
    // {
    //     yield return new WaitForSeconds(5);
    //     player.GetComponent<PlayerMovement>().enableMovement();
    //     animator.SetBool("IsEating", false);
    //     Destroy(carrot);

    // }

    // public void OnDeath()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
