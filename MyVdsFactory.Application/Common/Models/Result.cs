namespace MyVdsFactory.Application.Common.Models;

public class Result<T>
{
    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public string FriendlyMessage { get; set; }

    public T Data { get; set; }

    public static Result<T> Success( T data, string friendlyMessage = "")
    {
        return new Result<T>
        {
            Data = data,
            FriendlyMessage = friendlyMessage,
            Succeeded = true
        };
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>
        {
            Errors = errors.ToArray(),
            Succeeded = false
        };
    }
}