using System;
namespace Models.DTO
{
    public class seedInfoDto
    {
        public int nrSeededComments { get; set; } = 0;
        public int nrSeededAttractions { get; set; } = 0;
        public int nrSeededUsers { get; set; } = 0;
        public int nrSeededLocality { get; set; } = 0;

    }
    public class deleteSeedInfoDto
    {
        public int nrDeletedComments { get; set; } = 0;
        public int nrDeletedAttractions { get; set; } = 0;
        public int nrDeletedUsers { get; set; } = 0;
        public int nrDeletedLocality { get; set; } = 0;
    }
}

