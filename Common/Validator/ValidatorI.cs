namespace Common.Validator
{
    public interface ValidatorI<T>
    {
        ValDto Validate(T dto, Dictionary<string, string> labels);
        bool IsCodeUnique();
    }
}
