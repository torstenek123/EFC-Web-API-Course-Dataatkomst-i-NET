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

    [Table("Attractions", Schema = "supusr")]
    [Index(nameof(AttractionId))]
    [Index(nameof(Seeded))]
    [Index(nameof(Seeded),nameof(Category),nameof(Description),nameof(Name))]
    public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>, IEquatable<csAttractionDbM>
    {
        [Key]       // for EFC Code first
        public override Guid AttractionId { get; set; }

        [JsonIgnore]
        public virtual Guid LocalityId {get; set;}

        [Required]
        public override string Name {get; set;}

        [Required]
        public override enCategories Category {get; set;}
        
        public virtual string strCategory 
        {
            get => Category.ToString(); 
            set { }
        }
        
        [Required]
        public override string Description {get; set;}

        [NotMapped]
        public override ILocality Locality {get => LocalityDbM; set => new NotImplementedException();}

        [JsonIgnore]
        [ForeignKey("LocalityId")]
        public virtual csLocalityDbM LocalityDbM {get; set;}

        
        [NotMapped]
        public override List<IComment> Comments {get => CommentsDbM?.ToList<IComment>(); set => new NotImplementedException(); }

        //An attraction can have 0 or many Comments
        [JsonIgnore]
        public virtual List<csCommentDbM> CommentsDbM {get; set;} = null;

        #region helper methods
        public csAttractionDbM ExcludeNavProps() 
        {
            CommentsDbM = null;
            //LocalityDbM = null;
            return this;
        }
        public csAttractionDbM UpdateFromDTO(csAttractionCUdto org)
        {
            Name = org.Name;
            Category = org.Category;
            Description = org.Description;

            return this;
        }
        public csAttractionDbM(csAttractionCUdto org)
        {
            AttractionId = Guid.NewGuid();
            UpdateFromDTO(org);
        }

        #endregion

        #region implementing IEquatable
        public bool Equals(csAttractionDbM other) => (other != null) ? ((Comments, Category, Name, Description) ==
            (other.Comments, other.Category, other.Name, other.Description)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csAttractionDbM);

        public override int GetHashCode() => (Comments, Category, Name, Description).GetHashCode();
        #endregion

        #region randomly seed this instance
        public override csAttractionDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion


        #region constructors
        public csAttractionDbM() { }

        #endregion
    }
}

