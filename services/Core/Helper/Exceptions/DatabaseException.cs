namespace Core.Helper.Exceptions;

public class DatabaseException : BaseException
{
    public DatabaseException(string? message = null)
        : base(ErrorCode.Database, message)
    {
    }
}
