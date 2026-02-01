
namespace CoastalPharmacyCRUD.Helpers
{
    // Provides functions to use for any security proccess
    public static class SecurityHelpers
    {

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }


    }
}
