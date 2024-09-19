namespace Models{
    public interface IAttraction{
        public Guid AttractionId { get; set; }
        public string name {get;set;}
        public List<IComment> comments {get;set;}
        public string category {get;set;}

        public string description {get;set;}
        
        public ILocality locality {get;set;}

        
    }
}