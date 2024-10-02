using System;
using System.Diagnostics.Metrics;

using Seido.Utilities.SeedGenerator;
namespace Models
{
	public class csLocality : ILocality, ISeed<csLocality>, IEquatable<csLocality>
    {
        public virtual Guid LocalityId { get; set; }

        public virtual string StreetAddress { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public bool Seeded { get; set; } = false;
        public virtual List<IAttraction> Attractions { get; set; }

        public override string ToString() => $"{StreetAddress}, {ZipCode} {City}, {Country}";

        #region constructors
        public csLocality() { }
        public csLocality(csLocality org)
        {
            this.Seeded = org.Seeded;

            this.LocalityId = org.LocalityId;
            this.StreetAddress = org.StreetAddress;
            this.ZipCode = org.ZipCode;
            this.City = org.City;
            this.Country = org.Country;
        }
        #endregion

        #region implementing IEquatable

        public bool Equals(csLocality other) => (other != null) ? ((StreetAddress, ZipCode, City, Country) ==
            (other.StreetAddress, other.ZipCode, other.City, other.Country)) : false;

        public override bool Equals(object obj) => Equals(obj as csLocality);
        public override int GetHashCode() => (StreetAddress, ZipCode, City, Country, Seeded).GetHashCode();

        #endregion

        #region randomly seed this instance

        public virtual csLocality Seed(csSeedGenerator sgen)
        {
            LocalityId = Guid.NewGuid();
            Seeded = true;
            Country = sgen.Country;
            StreetAddress = sgen.StreetAddress(Country);
            ZipCode = sgen.ZipCode;
            City = sgen.City(Country);

            return this;
        }
        #endregion
    }
}

