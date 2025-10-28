using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroInteraction : MonoBehaviour
    {
        [Header("Основные настройки")] [SerializeField]
        private float pickupDistance = 3f;

        [SerializeField] private LayerMask interactableLayer;

        [Header("Физика удержания")] [SerializeField]
        private float holdDistance = 1.5f;

        [SerializeField] private float jointSpring = 2000f;
        [SerializeField] private float jointDamping = 80f;
        [SerializeField] private float maxForce = 2000f;

        [Header("Подсветка объекта")] [SerializeField]
        private Color highlightColor = Color.yellow;

        private Rigidbody heldObject;
        private ConfigurableJoint heldJoint;
        private bool isHolding;
        private Camera cam;

        private Renderer highlightedRenderer;
        private Color originalColor;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            HandleHighlight();

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (isHolding)
                    DropObject();
                else
                    TryPickupObject();
            }
        }

        private void FixedUpdate()
        {
            // Обновляем точку удержания (без дрожи)
            if (isHolding && heldJoint != null)
            {
                Vector3 targetAnchor = cam.transform.position + cam.transform.forward * holdDistance;
                heldJoint.connectedAnchor = Vector3.Lerp(heldJoint.connectedAnchor, targetAnchor, 0.5f);
            }
        }

        private void HandleHighlight()
        {
            if (isHolding)
            {
                ClearHighlight();
                return;
            }

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, interactableLayer))
            {
                Renderer r = hit.collider.GetComponent<Renderer>();
                if (r != null)
                {
                    if (highlightedRenderer != r)
                    {
                        ClearHighlight();
                        highlightedRenderer = r;
                        originalColor = r.material.color;
                        r.material.color = highlightColor;
                    }
                }
            }
            else
            {
                ClearHighlight();
            }
        }

        private void ClearHighlight()
        {
            if (highlightedRenderer != null)
            {
                highlightedRenderer.material.color = originalColor;
                highlightedRenderer = null;
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
                    ClearHighlight(); // убрать подсветку при подборе
                    heldObject = rb;
                    heldObject.useGravity = true;
                    heldObject.isKinematic = false;
                    heldObject.drag = 8f;
                    heldObject.angularDrag = 8f;
                    heldObject.constraints = RigidbodyConstraints.FreezeRotation;

                    heldJoint = heldObject.gameObject.AddComponent<ConfigurableJoint>();
                    heldJoint.autoConfigureConnectedAnchor = false;
                    heldJoint.connectedAnchor = cam.transform.position + cam.transform.forward * holdDistance;

                    heldJoint.xMotion = ConfigurableJointMotion.Limited;
                    heldJoint.yMotion = ConfigurableJointMotion.Limited;
                    heldJoint.zMotion = ConfigurableJointMotion.Limited;
                    heldJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    heldJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    heldJoint.angularZMotion = ConfigurableJointMotion.Locked;

                    JointDrive drive = new JointDrive
                    {
                        positionSpring = jointSpring,
                        positionDamper = jointDamping,
                        maximumForce = maxForce
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
                Destroy(heldJoint);

            if (heldObject != null)
            {
                heldObject.drag = 0f;
                heldObject.angularDrag = 0.05f;
                heldObject.constraints = RigidbodyConstraints.None;
                heldObject = null;
            }

            isHolding = false;
        }
    }
}
