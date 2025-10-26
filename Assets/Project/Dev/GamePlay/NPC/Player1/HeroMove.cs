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
        [SerializeField] private float rotationSpeed;
        private float _rotationAngle;
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

        void Update()
        {
            Move();
        }

        private void LateUpdate()
        {
             Rotation();
        }

        private void Move()
    {
        var camTransform = Camera.main.transform;

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
        if(_inputService.AimAxis.sqrMagnitude > 2f)
        {
            Vector2 aimVector = _inputService.AimAxis;
            _cinemachineService.Pov.m_HorizontalAxis.Value += aimVector.x * Time.fixedDeltaTime;
            _cinemachineService.Pov.m_VerticalAxis.Value -= aimVector.y * Time.fixedDeltaTime;
        }
    }

    }
}
