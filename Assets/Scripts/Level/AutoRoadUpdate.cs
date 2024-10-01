using PathCreation.Examples;
using UnityEngine;

namespace Butcher_TA
{
    [RequireComponent (typeof(RoadMeshCreator))]
    public class AutoRoadUpdate : MonoBehaviour
    {
        private RoadMeshCreator rmc;
        void Start()
        {
            rmc = GetComponent<RoadMeshCreator>();
            rmc.TriggerUpdate();
        }
    }
}
