namespace CommandLineBuilder
{
    public interface IConverter<T>
    {
        ConversionResult<T> Convert(string input);
    }
}
