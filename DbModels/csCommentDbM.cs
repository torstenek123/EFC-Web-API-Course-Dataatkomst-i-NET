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
    [Table("Comments", Schema = "supusr")]
    [Index(nameof(CommentId))]
    [Index(nameof(CommentId), nameof(UserId))]
    [Index(nameof(UserId), nameof(CommentId))]
    [Index(nameof(AttractionId), nameof(CommentId))]
    [Index(nameof(CommentId), nameof(AttractionId))]
    
    public class csCommentDbM : csComment, ISeed<csCommentDbM>, IEquatable<csCommentDbM>
    {
        [Key]
        public override Guid CommentId { get; set; }


        //A comment must have 1 User
        [JsonIgnore]
        public virtual Guid UserId { get; set; }

        //A comment must have 1 Attraction
        [JsonIgnore]
        public virtual Guid AttractionId { get; set; }

        [Required]
        public override string Comment {get;set;}

        [Required]
        [ForeignKey("AttractionId")]
        [JsonIgnore]
        public virtual csAttractionDbM AttractionDbM { get; set; }

        [NotMapped]
        public override IAttraction Attraction {get => AttractionDbM ; set => new NotImplementedException(); }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual csUserDbM UserDbM { get; set; } = null;

        [NotMapped]
        public override IUser User { get => UserDbM; set => new NotImplementedException(); }

        #region helper methods
        public csCommentDbM ExcludeNavProps() 
        {
            //UserDbM = null;
            AttractionDbM = null;
            return this;
        }
        public csCommentDbM ExcludeNavProps2() 
        {
            UserDbM = null;
            //AttractionDbM = null;
            return this;
        }

        public csCommentDbM UpdateFromDTO(csCommentCUdto org)
        {
            Comment = org.Comment;
            Date = DateTime.Now;
            return this;
        }
        public csCommentDbM(csCommentCUdto org)
        {
            CommentId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
        #endregion

        #region IEquatable
        public bool Equals(csCommentDbM other) => (other != null) ? ((Comment, Date, Attraction, User) ==
            (other.Comment, other.Date, other.Attraction, other.User)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csCommentDbM);

        public override int GetHashCode() => (Comment, Date, Attraction, User).GetHashCode();
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

