using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO
{
   
    public class csLocalityCUdto
    {
        public virtual Guid? LocalityId { get; set; }

        public virtual string StreetAddress { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual List<Guid> AttractionsId{ get; set; } = null;

        public csLocalityCUdto() { }
        public csLocalityCUdto(ILocality org)
        {
            LocalityId = org.LocalityId;
            StreetAddress = org.StreetAddress;
            ZipCode = org.ZipCode;
            City = org.City;
            Country = org.Country;
            AttractionsId = org.Attractions?.Select(a => a.AttractionId).ToList();
        }
    }

    public class csAttractionCUdto
    {
        public virtual Guid? AttractionId { get; set; } 
        public virtual string Name {get; set;}
        public virtual List<Guid> CommentsId {get;set;}
        public virtual enCategories Category {get; set;}
        public virtual string Description {get; set;}
        public virtual Guid? LocalityId {get; set;}
        public csAttractionCUdto(){}

        public csAttractionCUdto(IAttraction org)
        {
            AttractionId = org.AttractionId;
            Name = org.Name;
            Category = org.Category;
            Description = org.Description;
            LocalityId = org.Locality.LocalityId;

            CommentsId = org.Comments?.Select(c => c.CommentId).ToList();
        }
    }

    public class csUserCUdto 
    {
        public virtual Guid? UserId { get; set; } 

        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual enRoles Role { get; set; }

        public virtual List<Guid> CommentsId { get; set; }

        public csUserCUdto(){}
        public csUserCUdto(IUser org)
        {
            UserId = org.UserId;
            UserName = org.UserName;
            Email = org.Email;
            Password = org.Password;
            Role = org.Role;

            CommentsId = org.Comments?.Select(c => c.CommentId).ToList();
        }
    }

    public class csCommentCUdto
    {
        public virtual Guid? CommentId {get; set;} 
        public virtual string Comment {get; set;}
        public virtual DateTime? Date {get; init;} = DateTime.Now;
        public virtual Guid? AttractionId {get; set;}
        public virtual Guid? UserId {get; set;}

        public csCommentCUdto(){}
        public csCommentCUdto(IComment org)
        {
            CommentId = org.CommentId;
            Comment = org.Comment;
            //Date = org.Date;
            AttractionId = org.Attraction.AttractionId;
            UserId = org.User.UserId;
        }
    }

}

