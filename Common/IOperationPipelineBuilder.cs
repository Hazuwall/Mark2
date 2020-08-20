namespace Common
{
    public interface IOperationPipelineBuilder
    {
        IOperationPipelineBuilder AddPipe(IPipe pipe);
        IOperationPipelineBuilder AddPipe<T>() where T : IPipe;
    }
}
