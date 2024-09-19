using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO
{
    //DTO is a DataTransferObject, can be instanstiated by the controller logic
    //and represents a, fully instatiable, subset of the Database models
    //for a specific purpose.

    //These DTO are simplistic and used to Update and Create objects in the database
   
    public class csLocalityCUdto
    {
        public virtual Guid? LocalityId { get; set; }

        public virtual string StreetAddress { get; set; }
        public virtual int ZipCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }

        public csLocalityCUdto() { }
        public csLocalityCUdto(ILocality org)
        {
            LocalityId = org.LocalityId;
            StreetAddress = org.StreetAddress;
            ZipCode = org.ZipCode;
            City = org.City;
            Country = org.Country;
        }
    }


}

