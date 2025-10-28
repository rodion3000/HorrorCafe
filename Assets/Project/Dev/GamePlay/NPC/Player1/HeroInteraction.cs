using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroInteraction : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Физика удержания")]
    [SerializeField] private float holdDistance = 1.5f;
    [SerializeField] private float jointSpring = 2000f;   // сила "притяжения"
    [SerializeField] private float jointDamping = 80f;    // сглаживание
    [SerializeField] private float maxForce = 2000f;      // ограничение силы

    private Rigidbody heldObject;
    private ConfigurableJoint heldJoint;
    private bool isHolding;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
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
        // Обновляем положение якоря в FixedUpdate, чтобы не было jitter (дрожи)
        if (isHolding && heldJoint != null)
        {
            Vector3 targetAnchor = cam.transform.position + cam.transform.forward * holdDistance;
            heldJoint.connectedAnchor = Vector3.Lerp(heldJoint.connectedAnchor, targetAnchor, 0.5f);
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
                heldObject.drag = 8f;           // сглаживаем движение
                heldObject.angularDrag = 8f;
                heldObject.constraints = RigidbodyConstraints.FreezeRotation;

                // создаём joint на предмете
                heldJoint = heldObject.gameObject.AddComponent<ConfigurableJoint>();
                heldJoint.autoConfigureConnectedAnchor = false;
                heldJoint.connectedAnchor = cam.transform.position + cam.transform.forward * holdDistance;

                // ограничиваем движение, но не полностью фиксируем
                heldJoint.xMotion = ConfigurableJointMotion.Limited;
                heldJoint.yMotion = ConfigurableJointMotion.Limited;
                heldJoint.zMotion = ConfigurableJointMotion.Limited;
                heldJoint.angularXMotion = ConfigurableJointMotion.Locked;
                heldJoint.angularYMotion = ConfigurableJointMotion.Locked;
                heldJoint.angularZMotion = ConfigurableJointMotion.Locked;

                // устанавливаем мягкую “пружину” с ограничением силы
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
        {
            Destroy(heldJoint);
        }

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
