namespace YandexTracker;

/// <summary>
/// The base class of SDK-specific API and response serialization failures.
/// Argument validation, cancellation, timeouts and transport failures use standard .NET exceptions.
/// </summary>
public class YandexTrackerException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="YandexTrackerException"/> class.</summary>
    public YandexTrackerException() { }

    /// <summary>Initializes a new instance with an error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public YandexTrackerException(string message) : base(message) { }

    /// <summary>Initializes a new instance with an error message and the exception that caused it.</summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused this one.</param>
    public YandexTrackerException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>A response arrived but could not be read as the expected model.</summary>
public sealed class YandexTrackerSerializationException : YandexTrackerException
{
    /// <summary>Initializes a new instance of the <see cref="YandexTrackerSerializationException"/> class.</summary>
    public YandexTrackerSerializationException() { }

    /// <summary>Initializes a new instance with an error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public YandexTrackerSerializationException(string message) : base(message) { }

    /// <summary>Initializes a new instance with an error message and the serializer's exception.</summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The serializer's exception.</param>
    public YandexTrackerSerializationException(string message, Exception innerException) : base(message, innerException) { }
}
