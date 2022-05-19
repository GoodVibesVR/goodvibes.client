namespace GoodVibes.Client.Events
{
    public interface IDispatchEvent<in TEvent> where TEvent : IEvent
    {
        void Dispatch(TEvent e);
    }
}