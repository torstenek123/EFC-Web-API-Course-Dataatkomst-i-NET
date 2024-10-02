using Seido.Utilities.SeedGenerator;
namespace Models
{

    public class csComment: ISeed<csComment>, IComment{
        public virtual Guid CommentId {get; set;} 
        public virtual string Comment {get; set;}
        public virtual DateTime? Date {get; set;} 
        public override string ToString() => $"Comment: {Comment} Date: {Date:d}";

        public virtual IAttraction Attraction{get; set;}

        public virtual IUser User{get;set;}


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