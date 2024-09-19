using Seido.Utilities.SeedGenerator;
namespace Models
{

    public class csComment: ISeed<csComment>, IComment{
        public virtual Guid CommentId {get;set;} = Guid.NewGuid();
        public virtual string Comment {get;set;}
        public virtual DateTime Date {get;set;} 
        public override string ToString() => $"Comment: {Comment} Date: {Date:d}";

        //A comment must have 1 attraction
        public virtual IAttraction attraction{get; set;}

        //A comment can have 1 user
        public virtual IUser user{get;set;}


        #region Seeding
        public bool Seeded{get; set;} = false;
        public virtual csComment Seed(csSeedGenerator seed){
            Seeded = true;
            CommentId = Guid.NewGuid();
            Comment = seed.Quote.Quote;
            Date = seed.DateAndTime(2022, 2024);
            return this;
        }

        #endregion

    }


}