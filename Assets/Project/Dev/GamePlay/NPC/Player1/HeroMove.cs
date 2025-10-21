using Project.Dev.GamePlay.AnimatorLogic;
using Project.Dev.Services.CinemachineService;
using Project.Dev.Services.InputService;
using UniRx;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private int movementSpeed;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private float rotationSpeed;
        private float _rotationAngle;
        private IInputService _inputService;
        private bool _isTurningRightTriggered;
        private bool _isTurningLeftTriggered;
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
            TurnLogicSubscribe();
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
        var movementVector = Vector3.zero;

        if (_inputService.MoveAxis.sqrMagnitude > 0.001f)
        {
            var input = _inputService.MoveAxis;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                input.y = 0;
            else
                input.x = 0;

            float heroYAngle = transform.eulerAngles.y;
            Quaternion heroYRotation = Quaternion.Euler(0, heroYAngle, 0);

            movementVector = heroYRotation * new Vector3(input.x, 0, input.y);

            movementVector.y = 0;
            movementVector.Normalize();
        }

        movementVector += Physics.gravity;

        characterController.Move(movementVector * (movementSpeed * Time.deltaTime));

        Vector3 speed = characterController.velocity;
        speed.y = 0;
        if (speed.sqrMagnitude > 0.001f)
        {
            Vector3 localMove = transform.InverseTransformDirection(speed);
            float horizontalMove = localMove.x;
            float verticalMove = localMove.z;

            heroAnimator.PlayMove(Mathf.Abs(horizontalMove) > Mathf.Abs(verticalMove)
                ? (horizontalMove > 0 ? 2 : 3)
                : 1
            );
            _cinemachineService.SwitchToCamera(1);
        }
        else
        {
            heroAnimator.PlayMove(0);
            _cinemachineService.SwitchToCamera(2);
        }
    }

    private void Rotation()
    {
        Transform heroSpine = transform
            .Find("Model")
            .Find("GameSkeleton")
            .Find("Hips")
            .Find("Spine");

        Vector2 input = _inputService.AimAxis;
        if(_inputService.AimAxis.sqrMagnitude > 2f)
        {
            _rotationAngle += input.x * rotationSpeed * Time.deltaTime;
        }

        _rotationAngle = Mathf.Clamp(_rotationAngle, -45f, 45f);
        heroSpine.localRotation = Quaternion.Euler(-_rotationAngle, 5f, -10f);

        if (_rotationAngle >= 45f && !_isTurningRightTriggered)
        {
            heroAnimator.PlayTurnRight();
            _isTurningRightTriggered = true;
            _isTurningLeftTriggered = false;
        }
        else if (_rotationAngle <= -45f && !_isTurningLeftTriggered)
        {
            heroAnimator.PlayTurnLeft();
            _isTurningLeftTriggered = true;
            _isTurningRightTriggered = false;
        }
        else if (_rotationAngle > -45f && _rotationAngle < 45f)
        {
            _isTurningLeftTriggered = false;
            _isTurningRightTriggered = false;
        }
    }

    private void TurnLogicSubscribe()
    {
        Observable
            .FromEvent<AnimatorState>(
                x => heroAnimator.StateEntered += x,
                x => heroAnimator.StateEntered -= x)
            .Where(state => state == AnimatorState.TurnLeft || state == AnimatorState.TurnRight)
            .Subscribe(_ => TurnLogic());

    }

    private void TurnLogic()
    {
        Debug.Log("длолдо");

    }

    }
}
