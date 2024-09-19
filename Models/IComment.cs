namespace Models{
    public interface IComment{
        public Guid CommentId {get;set;}
        public string Comment {get;set;}
        public DateTime Date {get;set;}
        public IUser user {get;set;}
        public IAttraction attraction {get; set;}
    }
}