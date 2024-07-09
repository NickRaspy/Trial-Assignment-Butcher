using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRoadUpdate : MonoBehaviour
{
    private RoadMeshCreator rmc;
    void Start()
    {
        rmc = GetComponent<RoadMeshCreator>();
        rmc.TriggerUpdate();
    }
}
