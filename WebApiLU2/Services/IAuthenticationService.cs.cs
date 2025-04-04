namespace WebApiLU2.Services
{
    public interface IAuthenticationServices
    {
        /// <summary>
        /// Returns the user name of the authenticated user
        /// </summary>
        /// <returns></returns>
        string? GetCurrentAuthenticatedUserId();
    }
}
