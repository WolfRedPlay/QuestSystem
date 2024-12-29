using System;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    //----NEW-----
    [Serializable]
    [CreateAssetMenu(fileName = "ObjectiveReachPoint", menuName = "Objectives/ObjectiveReachPoint")]
    public class ObjectiveReachPoint : Objective
    {
        //----NEW-----
        [Tooltip("Object which will be created as a reach point")]
        [SerializeField] GameObject reachPointPrefab;

        //----NEW-----
        [Tooltip("The position where object will be created")]
        [SerializeField] Vector3 reachPointPosition;

        public override void OnAcceptance()
        {
            base.OnAcceptance();

            GameObject newReachPoint = Instantiate(reachPointPrefab, reachPointPosition, Quaternion.identity);
            newReachPoint.GetComponent<ReachPoint>().OnPointReached += FinishObjective;
        }


        public void FinishObjective()
        {
            CompleteObjective(string.Empty, string.Empty, "Objective complete ");
        }
    }

}