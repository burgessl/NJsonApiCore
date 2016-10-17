namespace NJsonApi
{
    public interface ILinkValueProvider
    {
        bool TryGetValue(string parameterName, out object value);
    }
}
