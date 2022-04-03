namespace McVerifier;

public class MessageSchemaValidationError
{
    public MessageSchemaValidationError(string kind, string property, string path, int lineNumber, int linePosition)
    {
        Kind = kind;
        Property = property;
        Path = path;
        LineNumber = lineNumber;
        LinePosition = linePosition;
    }

    //
    // Summary:
    //     Gets the error kind.
    public string Kind { get; }
    //
    // Summary:
    //     Gets the property name.
    public string Property { get; }
    //
    // Summary:
    //     Gets the property path.
    public string Path { get; }
    //
    // Summary:
    //     Gets the line number the validation failed on.
    public int LineNumber { get; }
    //
    // Summary:
    //     Gets the line position the validation failed on.
    public int LinePosition { get; }
}
