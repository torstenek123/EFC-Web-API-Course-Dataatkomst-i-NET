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
    [Table("Localities", Schema = "supusr")]
    public class csLocalityDbM : csLocality, ISeed<csLocalityDbM>, IEquatable<csLocalityDbM>
    {
        [Key]       // for EFC Code first
        public override Guid LocalityId { get; set; }

        public virtual List<csAttractionDbM> AttractionsDbM { get; set; }
        
        [Required]
        public override string StreetAddress { get; set; }
        [Required]
        public override int ZipCode { get; set; }
        [Required]
        public override string City { get; set; }
        [Required]
        public override string Country { get; set; }

        [NotMapped]
        public virtual List<IAttraction> attractions { get => AttractionsDbM?.ToList<IAttraction>() ; set => new NotImplementedException(); }
        
        #region implementing IEquatable
        public bool Equals(csLocalityDbM other) => (other != null) ?((StreetAddress, ZipCode, City, Country) ==
            (other.StreetAddress, other.ZipCode, other.City, other.Country)) :false;

        public override bool Equals(object obj) => Equals(obj as csLocalityDbM);
        public override int GetHashCode() => (StreetAddress, ZipCode, City, Country).GetHashCode();
        #endregion

        #region randomly seed this instance
        public override csLocalityDbM Seed(csSeedGenerator sgen)
        {
            base.Seed(sgen);
            return this;
        }
        #endregion


        #region constructors
        public csLocalityDbM() { }
        #endregion
    }
}

