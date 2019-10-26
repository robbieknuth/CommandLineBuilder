namespace CommandLine
{
    public interface IParser
    {
        IEntrypoint Parse(string[] args);
    }
}
