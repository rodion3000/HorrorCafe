using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroInteraction : MonoBehaviour
    {
        [SerializeField] private float pickupDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;

        private Rigidbody heldObject;
        private FixedJoint joint;
        private bool isHolding;

        void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (isHolding)
                    DropObject();
                else
                    TryPickupObject();
            }
        }

        private void TryPickupObject()
        {
            Transform camTransform = Camera.main.transform;
            Ray ray = new Ray(camTransform.position, camTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, interactableLayer))
            {
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    heldObject = rb;
                    heldObject.useGravity = false;
                    heldObject.isKinematic = false;
                    heldObject.velocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;

                    joint = camTransform.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = heldObject;
                    joint.breakForce = 1000f;
                    joint.breakTorque = 1000f;

                    isHolding = true;
                }
            }
        }

        private void DropObject()
        {
            if (joint != null)
            {
                Destroy(joint);
                joint = null;
            }

            if (heldObject != null)
            {
                heldObject.useGravity = true;
                heldObject = null;
                isHolding = false;
            }
        }
    }
}
