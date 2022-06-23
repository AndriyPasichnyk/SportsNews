using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews
{
    public static class AdminConfig
    {
        public const string Name = "AdminUserData";
        public const string UserEmail = "AdminUserData:Email";
        public const string Password = "AdminUserData:Password";
    }

    public class Claims
    {
        public const string FirstName = "First Name";
        public const string LastName = "Last Name";
        public const string FirstNameConfig = "FirstName";
        public const string LastNameConfig = "LastName";
    }

    public class ControllersNames
    {
        public const string Admin = "Admin";
        public const string Account = "Account";
        public const string Home = "Home";
        public const string Language = "Language";
    }

}
