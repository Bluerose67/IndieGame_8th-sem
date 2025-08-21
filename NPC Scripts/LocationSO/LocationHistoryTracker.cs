using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocationHistoryTracker : MonoBehaviour
{

    private readonly HashSet<LocationSO> locationsVisited = new HashSet<LocationSO>();

    public void RecordLocation(LocationSO locationSO)
    {
        if (locationsVisited.Add(locationSO))
        {
            
        }
    }

    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);
    }
}
