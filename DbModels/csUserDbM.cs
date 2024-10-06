using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

namespace DbModels
{   
    [Table("Users", Schema = "dbo")]
    [Index(nameof(UserId))]
    [Index(nameof(Seeded))]
    [Index(nameof(Seeded),nameof(Role), nameof(UserName))]
    public class csUserDbM : csUser, IEquatable<csUserDbM>, ISeed<csUserDbM>
	{
        [Key]     
        public override Guid UserId { get; set; }
        
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string Email { get; set; }
        [Required]
        public override string Password { get; set; }
        [Required]
        public override enRoles Role { get; set; }

        public virtual string strRole {get => Role.ToString(); set{}}

        //A user can have many Comments
        [JsonIgnore]
        public virtual List<csCommentDbM> CommentsDbM {get; set;} = null;
        
        [NotMapped]
        public override List<IComment> Comments {get => CommentsDbM?.ToList<IComment>() ;set => new NotImplementedException();}

        public csUserDbM ExcludeNavProps() 
        {
            Comments = null;
            return this;
        }
        public csUserDbM UpdateFromDTO(csUserCUdto org)
        {
            UserName = org.UserName;
            Email = org.Email;
            Password = org.Password;
            Role = org.Role;
            return this;
        }
        public csUserDbM(csUserCUdto org)
        {
            UserId = Guid.NewGuid();
            UpdateFromDTO(org);
        }

        
        #region IEquatable
        public bool Equals(csUserDbM other) => (other != null) ? ((Comments, Email, UserName) ==
            (other.Comments, other.Email, other.UserName)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csUserDbM);

        public override int GetHashCode() => (Comments, Email, UserName).GetHashCode();
        #endregion

        
        #region randomly seed this instance
        public override csUserDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion

        
        #region constructors
        public csUserDbM() { }
        
        #endregion
    }
}

