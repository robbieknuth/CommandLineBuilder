namespace CommandLineBuilder
{
    public interface IParser
    {
        IEntrypoint Parse(string[] args);
    }
}
