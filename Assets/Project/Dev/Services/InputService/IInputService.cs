using UnityEngine;
using UnityEngine.Events;

namespace Project.Dev.Services.InputService
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        Vector2 AimAxis { get; }

        event UnityAction AttackPressed;
    }
}
