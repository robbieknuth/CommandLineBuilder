namespace FluentCommandLine
{
    public interface IParser
    {
        IEntrypoint Parse(string[] args);
    }
}
