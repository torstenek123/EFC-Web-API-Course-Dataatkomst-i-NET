using System;
using Models;
using Models.DTO;

namespace Services{
    public interface IAttractionDataService{

        public Task<seedInfoDto> CreateSeed();
        public Task<deleteSeedInfoDto> RemoveSeed(bool seeded);

        public Task<IAttraction> ReadAttractionAsync(bool flat, Guid attractionId);
        public Task<IUser> ReadUserAsync(bool flat, Guid _userId);
        public Task<IComment> ReadCommentAsync(bool flat, Guid _commentId);
        public Task<ILocality> ReadLocalityAsync(bool flat, Guid _localityId);
        public Task<csRespPageDTO<IAttraction>> ReadAllAttractionsAsync(bool seeded, bool flat, int pageNumber, int pageSize, string filter);
        public Task<csRespPageDTO<IUser>> ReadAllUsersAsync(bool seeded, bool flat, int pageNr, int pageSize, string filter);
        public Task<csRespPageDTO<IAttraction>> ReadAllNoCommentsAsync(int pageNumber, int pageSize);

        public Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto);
        public Task<IUser> CreateUserAsync(csUserCUdto itemDto);
        public Task<IComment> CreateCommentAsync(csCommentCUdto itemDto);
        public Task<ILocality> CreateLocalityAsync(csLocalityCUdto itemDto);

        public Task<ILocality> UpdateLocality(csLocalityCUdto itemDto);
        public Task<IAttraction> UpdateAttraction(csAttractionCUdto itemDto);
        public Task<IUser> UpdateUser(csUserCUdto itemDto);
        public Task<IComment> UpdateComment(csCommentCUdto itemDto);

        public Task<ILocality> DeleteLocalityAsync(Guid localityId);
        public Task<IAttraction> DeleteAttraction(Guid attractionId);
        public Task<IUser> DeleteUser(Guid userId);
        public Task<IComment> DeleteComment(Guid commentId);
    }
}