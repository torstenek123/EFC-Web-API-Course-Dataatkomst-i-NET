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
    public class csAttractionDbM : csAttraction, ISeed<csAttractionDbM>, IEquatable<csAttractionDbM>
    {
        [Key]       // for EFC Code first
        public override Guid AttractionId { get; set; }

        [Required]
        [StringLength(50)]
        public override string name {get; set;}
        [Required]
        [StringLength(30)]
        public override string category {get; set;}
        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public override string description {get; set;}

        [NotMapped]
        public override ILocality locality {get => LocalityDbM; set => new NotImplementedException();}

        [JsonIgnore]
        [ForeignKey("LocalityId")]
        public virtual csLocalityDbM LocalityDbM {get; set;}

        public virtual Guid LocalityId {get; set;}

        
        [NotMapped]
        public override List<IComment> comments {get => CommentsDbM?.ToList<IComment>(); set => new NotImplementedException(); }

        //An attraction can have 0 or many comments
        [JsonIgnore]
        public virtual List<csCommentDbM> CommentsDbM {get; set;} = null;

        #region implementing IEquatable
        public bool Equals(csAttractionDbM other) => (other != null) ? ((comments, category, name, description) ==
            (other.comments, other.category, other.name, other.description)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csAttractionDbM);

        public override int GetHashCode() => (comments, category, name, description).GetHashCode();
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

