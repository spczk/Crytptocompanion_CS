using System;
namespace Cryptocompanion
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string RecoveryCode { get; set; }
        public User()
        {
        }
    }
}
