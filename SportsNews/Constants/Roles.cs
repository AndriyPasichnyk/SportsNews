namespace SportsNews
{
    public static class Roles
    {
        public const string Administrator = "Admin";
        public const string User = "User";
    }

    public static class Policies
    {
        public const string Admins = "AdminsOnly";
        public const string All = "AllUsers";
    }
}
