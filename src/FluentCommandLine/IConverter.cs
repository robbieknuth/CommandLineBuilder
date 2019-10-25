namespace FluentCommandLine
{
    public interface IConverter<T>
    {
        ConversionResult<T> Convert(string input);
    }
}
