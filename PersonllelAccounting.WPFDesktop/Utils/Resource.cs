namespace WPF_Desktop.Utils;

public class Resource<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? Message { get; private set; }
    public string? ExceptionMessage { get; private set; }

    public static Resource<T> Success(T? data) =>
        new()
        {
            IsSuccess = true,
            Data = data
        };

    public static Resource<T> Failure(string message, Exception exception) =>
        new()
        {
            IsSuccess = false,
            Message = message,
            ExceptionMessage =  exception.Message
        };
}

public class Resource
{
    public bool IsSuccess { get; private set; }
    public string? Message { get; private set; }
    public string? ExceptionMessage { get; private set; }

    public static Resource Success() =>
        new() { IsSuccess = true };

    public static Resource Failure(string message, Exception exception) =>
        new()
        {
            IsSuccess = false,
            Message = message,
            ExceptionMessage = exception.Message
        };
}