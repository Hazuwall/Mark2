namespace Common
{
    public interface IOperationPipelineBuilder
    {
        IOperationPipelineBuilder AddPipe(IPipe pipe);
    }
}
