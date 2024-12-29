using Unity.FPS.Gameplay;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectivePickupItem : Objective
    {
        [Tooltip("Item to pickup to complete the objective")]
        public GameObject ItemToPickup;

        public override void OnAcceptance()
        {
            base.OnAcceptance();

            EventManager.AddListener<PickupEvent>(OnPickupEvent);
        }

        void OnPickupEvent(PickupEvent evt)
        {
            if (IsCompleted || ItemToPickup != evt.Pickup)
                return;

            // this will trigger the objective completion
            // it works even if the player can't pickup the item (i.e. objective pickup healthpack while at full heath)
            CompleteObjective(string.Empty, string.Empty, "Objective complete : ");

            //if (gameObject)
            //{
            //    Destroy(gameObject);
            //}
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PickupEvent>(OnPickupEvent);
        }
    }
}