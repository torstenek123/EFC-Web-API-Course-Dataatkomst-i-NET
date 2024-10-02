namespace Models{
    public interface IAttraction{
        public Guid AttractionId { get; set; }
        public string Name {get;set;}
        public List<IComment> Comments {get;set;}
        public enCategories Category {get;set;}

        public string Description {get;set;}
        
        public ILocality Locality {get;set;}

        
    }
}