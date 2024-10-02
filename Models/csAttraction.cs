using System;
using System.Diagnostics.Metrics;

using Seido.Utilities.SeedGenerator;
namespace Models{
    public enum enCategories{
        Park, Slop, Café, Restaurant, Museum, Beach, Bar, Hotel, Library, Zoo, Garden 
    }

    public class csAttraction : IAttraction, ISeed<csAttraction>, IEquatable<csAttraction>{

        public virtual Guid AttractionId { get; set; } = Guid.NewGuid();
        public virtual string Name {get; set;}
        public virtual List<IComment> Comments {get;set;}
        public virtual enCategories Category {get; set;}
        public virtual string Description {get; set;}
        public virtual ILocality Locality {get; set;}
        

        //save for after debug if (Comments!=null) Comments.ForEach(c => retStr += $"\n{c.ToString()}");
        public override string ToString()  {
            string retStr = $"{Name}\nCategory: {Category}\nDescription: {Description}\nComments:";
            string strComments = "";
            if (Comments!=null)
            {
                foreach(var comment in Comments) {
                    strComments += $"\n{comment.ToString()}";
                }
                retStr += strComments;
            }
            return retStr;
        }
        public csAttraction(){}

        #region Seeding
        public bool Seeded{ get; set;} = false;
        public virtual csAttraction Seed(csSeedGenerator sgen)
        {

            Seeded = true;
            AttractionId = Guid.NewGuid();
            Category = sgen.FromEnum<enCategories>();
            Name = $"{sgen.FullName}'s {sgen.Adjective} {Category}";
            string oneLatinSentenceOrWrd = sgen.Bool ? $"sentence: {sgen.LatinSentence}" : $"word: {sgen.LatinWords(1).FirstOrDefault()}";
            Description = $"{Name} can be described in one {oneLatinSentenceOrWrd}";

            return this;
        }
        #endregion

        #region IEquatable
        public bool Equals(csAttraction other) => (other != null) ? ((Comments, Category, Name, Description) ==
            (other.Comments, other.Category, other.Name, other.Description)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csAttraction);

        public override int GetHashCode() => (Comments, Category, Name, Description).GetHashCode();
        #endregion
    }
}