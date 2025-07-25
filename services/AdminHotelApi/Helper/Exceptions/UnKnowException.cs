namespace AdminHotelApi.Helper.Exceptions;

public class UnKnowException : BaseException
{
    public UnKnowException(string? message = null)
        : base(ErrorCode.UnKnow, message)
    {
    }
}
