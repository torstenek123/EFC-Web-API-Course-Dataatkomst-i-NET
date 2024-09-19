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
    public class csUserDbM : csUser, IEquatable<csUserDbM>, ISeed<csUserDbM>
	{
        [Key]     
        public override Guid UserId { get; set; }

        //A user can have 0 or many comments
        [JsonIgnore]
        public virtual List<csCommentDbM> CommentsDbM {get; set;} = null;
        
        [NotMapped]
        public override List<IComment> comments {get => CommentsDbM?.ToList<IComment>() ;set => new NotImplementedException();}
        
        #region IEquatable
        public bool Equals(csUserDbM other) => (other != null) ? ((comments, Email, UserName) ==
            (other.comments, other.Email, other.UserName)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csUserDbM);

        public override int GetHashCode() => (comments, Email, UserName).GetHashCode();
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

