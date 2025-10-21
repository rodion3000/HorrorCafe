using System;
using Project.Dev.GamePlay.AnimatorLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Dev.GamePlay.NPC.Player1
{
    public class HeroAnimator : MonoBehaviour, IAnimatorStateReader
    {
        // animator params
        private static readonly int MoveDirection = Animator.StringToHash("MoveDirection");
        private static readonly int TurnLeftHash = Animator.StringToHash("TurnLeft");
        private static readonly int TurnRightHash = Animator.StringToHash("TurnRight");

        //animator states
        private readonly int _moveDirection = Animator.StringToHash("MoveDirection");
        private readonly int _turnLeftHash = Animator.StringToHash("TurnLeft");
        private readonly int _turnRightHash = Animator.StringToHash("TurnRight");

        [SerializeField] private Animator animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public AnimatorState State { get; private set; }

        #region Play methods

        [HorizontalGroup("Actions")]
        [Button("PlayMove"), GUIColor(0, 0, 1)]
        public void PlayMove(int value) =>
            animator.SetInteger(MoveDirection, value);

        [HorizontalGroup("Actions")]
        [Button ("TurnLeft"), GUIColor(0.5f, 0.5f, 0)]
        public void PlayTurnLeft() =>
            animator.SetTrigger(TurnLeftHash);

        [HorizontalGroup("Actions")]
        [Button ("TurnRight"), GUIColor(1f, 0f, 0)]
        public void PlayTurnRight() =>
            animator.SetTrigger(TurnRightHash);

        #endregion

        public void OnEnter(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void OnExit(int stateHash)
        {
            StateExited?.Invoke(State);
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if (stateHash == _moveDirection)      state = AnimatorState.MoveDirection;
            else if (stateHash == _turnRightHash)    state = AnimatorState.TurnRight;
            else if (stateHash == _turnLeftHash)     state = AnimatorState.TurnLeft;
            else                                     state = AnimatorState.Unknow;

            return state;
        }

    }
}
