namespace TechnicalTest.Services.Exceptions
{
    public class RoleDoesNotExistException : Exception
    {
        public RoleDoesNotExistException(string message) : base(message)
        {
        }
    }
}