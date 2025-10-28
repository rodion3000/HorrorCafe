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
    [SerializeField] private float jointSpring = 2000f;
    [SerializeField] private float jointDamping = 80f;
    [SerializeField] private float maxForce = 2000f;

    [Header("Подсветка объекта")]
    [SerializeField] private Color highlightColor = Color.yellow;

    private Camera cam;
    private Rigidbody heldObject;
    private ConfigurableJoint heldJoint;
    private Renderer highlightedRenderer;
    private Color originalColor;

    private bool IsHolding => heldObject != null;

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        HandleHighlight();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (IsHolding) DropObject();
            else TryPickupObject();
        }
    }

    private void FixedUpdate()
    {
        if (!IsHolding || !heldJoint) return;

        Vector3 targetAnchor = cam.transform.position + cam.transform.forward * holdDistance;
        heldJoint.connectedAnchor = Vector3.Lerp(heldJoint.connectedAnchor, targetAnchor, 0.5f);
    }

    private void HandleHighlight()
    {
        if (IsHolding) { ClearHighlight(); return; }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, pickupDistance, interactableLayer))
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r && highlightedRenderer != r)
            {
                ClearHighlight();
                highlightedRenderer = r;
                originalColor = r.material.color;
                r.material.color = highlightColor;
            }
        }
        else ClearHighlight();
    }

    private void ClearHighlight()
    {
        if (!highlightedRenderer) return;
        highlightedRenderer.material.color = originalColor;
        highlightedRenderer = null;
    }

    private void TryPickupObject()
    {
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, pickupDistance, interactableLayer))
            return;

        Rigidbody rb = hit.rigidbody;
        if (!rb) return;

        ClearHighlight();
        heldObject = rb;
        SetupHeldObject(rb);
        CreateJoint(rb);
    }

    private void SetupHeldObject(Rigidbody rb)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.drag = 8f;
        rb.angularDrag = 8f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void CreateJoint(Rigidbody rb)
    {
        heldJoint = rb.gameObject.AddComponent<ConfigurableJoint>();
        heldJoint.autoConfigureConnectedAnchor = false;
        heldJoint.connectedAnchor = cam.transform.position + cam.transform.forward * holdDistance;

        heldJoint.xMotion = heldJoint.yMotion = heldJoint.zMotion = ConfigurableJointMotion.Limited;
        heldJoint.angularXMotion = heldJoint.angularYMotion = heldJoint.angularZMotion = ConfigurableJointMotion.Locked;

        var drive = new JointDrive
        {
            positionSpring = jointSpring,
            positionDamper = jointDamping,
            maximumForce = maxForce
        };

        heldJoint.xDrive = heldJoint.yDrive = heldJoint.zDrive = drive;
    }

    private void DropObject()
    {
        if (heldJoint) Destroy(heldJoint);

        if (heldObject)
        {
            heldObject.drag = 0f;
            heldObject.angularDrag = 0.05f;
            heldObject.constraints = RigidbodyConstraints.None;
            heldObject = null;
        }
    }
}
}
