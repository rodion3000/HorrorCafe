
namespace Project.Dev.GamePlay.AnimatorLogic
{
    public interface IAnimatorStateReader
    {
        AnimatorState State { get; }
        void OnEnter(int stateHash);
        void OnExit(int stateHash);
        
    }
}
