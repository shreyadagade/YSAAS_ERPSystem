namespace UserManagement.Application.Exceptions
{
    public class EmailException : AppException
    {
        public EmailException(string message)
            : base(message, 500)
        {
        }
    }
}