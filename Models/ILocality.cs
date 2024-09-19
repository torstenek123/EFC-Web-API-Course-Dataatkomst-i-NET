
namespace Models
{
	public interface ILocality 
    {
        public Guid LocalityId { get; set; }

        public string StreetAddress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public List<IAttraction> attractions { get; set; } 

    
    }
}

