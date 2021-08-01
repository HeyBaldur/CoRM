using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Models.V1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Username { get; set; }
        public bool AccountConfirmed { get; set; }
        public string PhotoUrl { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsLogged { get; set; }
        public string Uid { get; set; }

        public User()
        {
            EnrollmentDate = DateTime.Now;
        }
    }
}
