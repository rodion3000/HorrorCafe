
namespace Project.Dev.GamePlay.Items.Interface
{
    public interface IItemEventHandler<T> where T: IItemEvent
    {
        void Handle(T evt);
    }
}
