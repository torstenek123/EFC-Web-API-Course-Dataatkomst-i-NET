using System;
using Configuration;
using Seido.Utilities.SeedGenerator;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace Models
{
    public enum enRoles{sysadmin, gstuser, usr, supusr}
	public class csUser : IUser, ISeed<csUser>, IEquatable<csUser>
	{
        public virtual Guid UserId { get; set; } = Guid.NewGuid();

        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual enRoles Role { get; set; }
        

        public override string ToString() => $"Username: {UserName}\nEmail: {Email}\nPassword: {Password}\nRole: {Role}\nSeeded: {Seeded}";


        public virtual List<IComment> Comments { get; set; }
        public csUser(){

        }

        #region Seeding
        public bool Seeded { get; set; } = false;

        public virtual csUser Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            string fullName = $"{sgen.FirstName}{sgen.LastName}";
            UserName = $"{fullName}{sgen.Next(0,100)}";
            Email = sgen.Email(fullName);
            Role = sgen.FromEnum<enRoles>();
            Password = EncryptPassword($"{UserName}+{Email}+{Role}");
            return this;
        }

        private static string EncryptPassword(string _password)
        {
            //Hash a password using salt and streching
            byte[] encrypted = KeyDerivation.Pbkdf2(
                password: _password,
                salt: Encoding.UTF8.GetBytes(csAppConfig.PasswordSalt.Salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: csAppConfig.PasswordSalt.Iterations,
                numBytesRequested: 64);

            return Convert.ToBase64String(encrypted);
        }
        #endregion

        #region IEquatable
        public bool Equals(csUser other) => (other != null) ? ((Comments, Email, UserName) ==
            (other.Comments, other.Email, other.UserName)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csUser);

        public override int GetHashCode() => (Comments, Email, UserName).GetHashCode();
        #endregion
    }
}

