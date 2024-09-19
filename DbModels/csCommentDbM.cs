using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

using Seido.Utilities.SeedGenerator;
using Configuration;
using Models;
using Models.DTO;


namespace DbModels
{
    public class csCommentDbM : csComment, ISeed<csCommentDbM>, IEquatable<csCommentDbM>
    {
        [Key]
        public override Guid CommentId { get; set; }


        //A comment can have a user
        [JsonIgnore]
        public virtual Guid? UserId { get; set; }

        //A comment must have an attraction
        [JsonIgnore]
        public virtual Guid AttractionId { get; set; }

        [Required]
        public override string Comment {get;set;}

        [Required]
        public override DateTime Date {get;set;} 

        [Required]
        [JsonIgnore]
        [ForeignKey("AttractionId")]
        public virtual csAttractionDbM AttractionDbM { get; set; }

        [NotMapped]
        public override IAttraction attraction {get => AttractionDbM ; set => new NotImplementedException(); }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual csUserDbM UserDbM { get; set; } = null;

        [NotMapped]
        public override IUser user { get => UserDbM; set => new NotImplementedException(); }


        #region IEquatable
        public bool Equals(csCommentDbM other) => (other != null) ? ((Comment, Date, attraction, user) ==
            (other.Comment, other.Date, other.attraction, other.user)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csCommentDbM);

        public override int GetHashCode() => (Comment, Date, attraction, user).GetHashCode();
        #endregion

        #region randomly seed this instance
        public override csCommentDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion


        #region constructors
        public csCommentDbM() { }

        #endregion
    }
}

