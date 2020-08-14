namespace Common
{
    public interface IPipe
    {
        void Process(OperationContext transaction);
    }
}
