using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueHistoryTracker : MonoBehaviour
{
   
    private readonly HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();


    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
