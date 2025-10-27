using Project.Dev.Services.CinemachineService;
using Project.Dev.Services.InputService;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroMove : MonoBehaviour
{
    [SerializeField] private int movementSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float rotationSpeed ;

    private IInputService _inputService;
    private ICinemachineService _cinemachineService;

    [Inject]
    private void Construct(IInputService inputService, ICinemachineService cinemachineService)
    {
        _inputService = inputService;
        _cinemachineService = cinemachineService;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void Move()
    {
        Transform camTransform = Camera.main.transform;

        Vector3 forwardVector = camTransform.forward;
        forwardVector.y = 0;
        forwardVector.Normalize();

        Vector3 rightVector = camTransform.right;
        rightVector.y = 0;
        rightVector.Normalize();

        Vector3 movementVector =
            (forwardVector * _inputService.MoveAxis.y + rightVector * _inputService.MoveAxis.x).normalized;

        movementVector += Physics.gravity;
        characterController.Move(movementVector * (movementSpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        Vector2 rotationAxis = _inputService.AimAxis;
        if (rotationAxis.sqrMagnitude > 2f && _cinemachineService.Pov != null)
        {
            _cinemachineService.Pov.m_HorizontalAxis.Value += rotationAxis.x * rotationSpeed * Time.deltaTime;
            _cinemachineService.Pov.m_VerticalAxis.Value -= rotationAxis.y * rotationSpeed * Time.deltaTime;
        }
    }
}
}
