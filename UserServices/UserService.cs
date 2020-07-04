using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSamples
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

        bool IsValidUser(string username, string password);
    }

    public class UserService : IUserService
    {
        // get these from a db or whereever...
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Marcel", LastName = "Heeremans", Username = "marcel", Password = "abc001" }
        };

        /// <summary>
        /// Async user cred check 
        /// </summary>
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            user.Password = "";
            return user;
        }

        // synchronous user cred check... (just different ways)
        public bool IsValidUser(string username, string password)
        {
            if (username == "marcel" && password == "abc001") return true;
            return false;
        }

    }

}
