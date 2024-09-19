using System;

using Seido.Utilities.SeedGenerator;

namespace Models
{
	public class csUser : IUser, ISeed<csUser>, IEquatable<csUser>
	{
        public virtual Guid UserId { get; set; } = Guid.NewGuid();

        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
    

        
        public virtual List<IComment> comments { get; set; }
        public csUser(){

        }

        #region Seeding
        public bool Seeded { get; set; } = false;

        public virtual csUser Seed(csSeedGenerator sgen)
        {
            Seeded = true;
            UserName = sgen.FullName;
            Email = sgen.Email(UserName);
            return this;
        }
        #endregion

        #region IEquatable
        public bool Equals(csUser other) => (other != null) ? ((comments, Email, UserName) ==
            (other.comments, other.Email, other.UserName)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csUser);

        public override int GetHashCode() => (comments, Email, UserName).GetHashCode();
        #endregion
    }
}

