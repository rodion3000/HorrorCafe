using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroInteraction : MonoBehaviour
    {
        [SerializeField] private float pickupDistance = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float jointSpring = 5000f; // сила притяжения
        [SerializeField] private float jointDamping = 100f; // сглаживание

        private Rigidbody heldObject;
        private ConfigurableJoint heldJoint;
        private bool isHolding;
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (isHolding)
                    DropObject();
                else
                    TryPickupObject();
            }

            // обновляем точку привязки (камера)
            if (isHolding && heldJoint != null)
            {
                heldJoint.connectedAnchor = cam.transform.position + cam.transform.forward * 1.5f;
            }
        }

        private void TryPickupObject()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, interactableLayer))
            {
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    heldObject = rb;
                    heldObject.useGravity = true;
                    heldObject.isKinematic = false;

                    // создаём joint НА ПРЕДМЕТЕ
                    heldJoint = heldObject.gameObject.AddComponent<ConfigurableJoint>();
                    heldJoint.connectedBody = null; // соединяем не с rigidbody, а с world anchor
                    heldJoint.autoConfigureConnectedAnchor = false;
                    heldJoint.connectedAnchor = cam.transform.position + cam.transform.forward * 1.5f;

                    // блокируем вращение, но оставляем движение
                    heldJoint.xMotion = ConfigurableJointMotion.Limited;
                    heldJoint.yMotion = ConfigurableJointMotion.Limited;
                    heldJoint.zMotion = ConfigurableJointMotion.Limited;
                    heldJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    heldJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    heldJoint.angularZMotion = ConfigurableJointMotion.Locked;

                    // настраиваем мягкую пружину
                    JointDrive drive = new JointDrive
                    {
                        positionSpring = jointSpring,
                        positionDamper = jointDamping,
                        maximumForce = Mathf.Infinity
                    };

                    heldJoint.xDrive = drive;
                    heldJoint.yDrive = drive;
                    heldJoint.zDrive = drive;

                    isHolding = true;
                }
            }
        }

        private void DropObject()
        {
            if (heldJoint != null)
            {
                Destroy(heldJoint);
            }

            if (heldObject != null)
            {
                heldObject.useGravity = true;
                heldObject = null;
            }

            isHolding = false;
        }
    }
}
