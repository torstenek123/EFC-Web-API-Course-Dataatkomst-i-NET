using System;
using System.Diagnostics.Metrics;

using Seido.Utilities.SeedGenerator;
namespace Models{
    public enum categories{
        Park, Slop, Café, Restaurant, Museum, Beach, Bar, Hotel, Library, Zoo, Garden 
    }
    public enum adjectives {
        Calm, Bright, Cozy, Spacious, Elegant, Vibrant, Quiet, 
        Charming, Lively, Serene, Bold, Warm, Sleek, Modern,
    }
    public class csAttraction : IAttraction, ISeed<csAttraction>, IEquatable<csAttraction>{

        public virtual Guid AttractionId { get; set; } = Guid.NewGuid();
        public virtual string name {get; set;}
        public virtual List<IComment> comments {get;set;}
        public virtual string category {get; set;}
        public virtual string description {get; set;}
        public virtual ILocality locality {get; set;}
        

        //save for after debug if (comments!=null) comments.ForEach(c => retStr += $"\n{c.ToString()}");
        public override string ToString()  {
            string retStr = $"{name}\nCategory: {category}\nDescription: {description}\nComments:";
            string strComments = "";
            if (comments!=null)
            {
                foreach(var comment in comments) {
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
            category = sgen.FromEnum<categories>().ToString();
            name = $"{sgen.FullName}'s {sgen.FromEnum<adjectives>()} {category}";
            string oneLatinSentenceOrWrd = sgen.Bool ? $"sentence: {sgen.LatinSentence}" : $"word: {sgen.LatinWords(1).FirstOrDefault()}";
            description = $"{name} can be described in one {oneLatinSentenceOrWrd}";

            return this;
        }
        #endregion

        #region IEquatable
        public bool Equals(csAttraction other) => (other != null) ? ((comments, category, name, description) ==
            (other.comments, other.category, other.name, other.description)) : false;
        
        public override bool Equals(object obj) => Equals(obj as csAttraction);

        public override int GetHashCode() => (comments, category, name, description).GetHashCode();
        #endregion
    }
}