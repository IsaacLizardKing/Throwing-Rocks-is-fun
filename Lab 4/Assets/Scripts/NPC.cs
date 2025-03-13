using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int repeatStartPosition;

    public string npcName;
    public DialogueAsset dialogueAsset;
    private int position = 0;

    public int getPosition()
    {
        return position;
    }

    public void incPosition()
    {
        position += 1;
    }
    // [HideInInspector]
    // public int StartPosition
    // {
    //     get
    //     {
    //         if (firstInteraction)
    //         {
    //             firstInteraction = false;
    //             return 0;
    //         }
    //         else
    //         {
    //             return repeatStartPosition;
    //         }
    //     }
    // }

    internal static void Destroy()
    {
        throw new NotImplementedException();
    }
}
