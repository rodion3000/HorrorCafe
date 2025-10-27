using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroInteraction : MonoBehaviour
    {
        [SerializeField] private Transform holdPoint;
        [SerializeField] private float pickupDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;

        private Rigidbody heldObject;
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
                    heldObject.isKinematic = true;
                    heldObject.transform.SetParent(camTransform);
                    isHolding = true;
                }
            }
        }

        private void DropObject()
        {
            if (heldObject != null)
            {
                heldObject.isKinematic = false;
                heldObject.transform.SetParent(null);
                heldObject = null;
                isHolding = false;
            }
        }
    }
}
