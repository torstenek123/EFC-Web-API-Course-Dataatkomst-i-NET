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
    [Index(nameof(LocalityId))]
    public class csLocalityDbM : csLocality, ISeed<csLocalityDbM>, IEquatable<csLocalityDbM>
    {
        [Key]       // for EFC Code first
        public override Guid LocalityId { get; set; }
        
        [Required]
        public override string StreetAddress { get; set; }

        [Required]
        public override string City { get; set; }

        [Required]
        public override string Country { get; set; }

        [JsonIgnore]
        public virtual List<csAttractionDbM> AttractionsDbM { get; set; } = null;

        [NotMapped]
        public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }

         public csLocalityDbM ExcludeNavProps() 
        {
            AttractionsDbM = null;
            return this;
        }
        public csLocalityDbM UpdateFromDTO(csLocalityCUdto org)
        {
            StreetAddress = org.StreetAddress;
            ZipCode = org.ZipCode;
            City = org.City;
            Country = org.Country;
            

            return this;
        }
        public csLocalityDbM(csLocalityCUdto org)
        {
            LocalityId = Guid.NewGuid();
            UpdateFromDTO(org);
        }
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

