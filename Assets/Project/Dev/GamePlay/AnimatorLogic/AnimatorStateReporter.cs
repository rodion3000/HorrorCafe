using UnityEngine;

namespace Project.Dev.GamePlay.AnimatorLogic
{
    [DisallowMultipleComponent]
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimatorStateReader _stateReader;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            FindReader(animator).OnEnter(stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindReader(animator).OnExit(stateInfo.shortNameHash);
        }

        private IAnimatorStateReader FindReader(Animator animator) =>
            _stateReader ??= animator.transform.parent.GetComponent<IAnimatorStateReader>();

    }
}
