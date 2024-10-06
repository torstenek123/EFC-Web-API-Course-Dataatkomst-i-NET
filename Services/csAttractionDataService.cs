using System;
using DbContext;
using DbRepos;
using Microsoft.Extensions.Logging;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;
using Microsoft.AspNetCore.JsonPatch.Internal;

namespace Services
{
    public class csAttractionDataService : IAttractionDataService
    {        
        //Services injected
        private ILogger<csAttractionDataService> _logger = null;
        private csAttractionDbRepos _repo = null;


        #region only for layer verification
    
        private Guid _guid = Guid.NewGuid();
        private string _instanceHeartbeat;

        public static string Heartbeat {get;} = $"Heartbeat from namespace {nameof(Services)}, class {nameof(csAttractionDataService)}";
        public csAttractionDataService()
        {
            //only for layer verification
            _instanceHeartbeat = $"Heartbeat from class {this.GetType()} with instance Guid {_guid}. ";
        }
        #endregion
        
        #region methods
        public Task<ILocality> DeleteLocalityAsync(Guid localityId) => _repo.DeleteLocalityAsync(localityId);
        public Task<IAttraction> DeleteAttraction(Guid attractionId) => _repo.DeleteAttractionAsync(attractionId);

        public Task<IUser> DeleteUser(Guid userId) => _repo.DeleteUserAsync(userId);

        public Task<IComment> DeleteComment(Guid commentId) => _repo.DeleteCommentAsync(commentId);

        public Task<IAttraction> ReadAttractionAsync(bool flat, Guid attractionId) => _repo.ReadAttractionAsync(flat, attractionId);
        public Task<IUser> ReadUserAsync(bool flat, Guid _userId) => _repo.ReadUserAsync(flat, _userId);
        public Task<IComment> ReadCommentAsync(bool flat, Guid _commentId) => _repo.ReadCommentAsync(flat, _commentId);
        public Task<ILocality> ReadLocalityAsync(bool flat, Guid _localityId) => _repo.ReadLocalityAsync(flat, _localityId);

        public Task<csRespPageDTO<IAttraction>> ReadAllNoCommentsAsync(int pageNumber, int pageSize) => _repo.ReadAllNoCommentsAsync(pageNumber, pageSize);
        public Task<csRespPageDTO<IAttraction>> ReadAllAttractionsAsync(bool seeded, bool flat, int pageNumber, int pageSize, string filter)  

                    => _repo.ReadAllAttractionsAsync(seeded, flat, pageNumber, pageSize, filter);
        public Task<csRespPageDTO<IUser>> ReadAllUsersAsync(bool seeded, bool flat, int pageNr, int pageSize, string filter) 
                    => _repo.ReadAllUsersAsync(seeded,  flat,  pageNr, pageSize, filter);
        
        public Task<seedInfoDto> CreateSeed() => _repo.CreateSeedAsync();
        public Task<deleteSeedInfoDto> RemoveSeed(bool seeded) => _repo.RemoveSeedAsync(seeded);

        public Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto) => _repo.CreateAttractionAsync(itemDto);
        public Task<IUser> CreateUserAsync(csUserCUdto itemDto) => _repo.CreateUserAsync(itemDto);
        public Task<IComment> CreateCommentAsync(csCommentCUdto itemDto) => _repo.CreateCommentAsync(itemDto);
        public Task<ILocality> CreateLocalityAsync(csLocalityCUdto itemDto) => _repo.CreateLocalityAsync(itemDto);


        public Task<ILocality> UpdateLocality(csLocalityCUdto itemDto) => _repo.UpdateLocality(itemDto);
        public Task<IAttraction> UpdateAttraction(csAttractionCUdto itemDto) => _repo.UpdateAttraction(itemDto);
        public Task<IUser> UpdateUser(csUserCUdto itemDto) => _repo.UpdateUser(itemDto);
        public Task<IComment> UpdateComment(csCommentCUdto itemDto) => _repo.UpdateComment(itemDto);


    





        #endregion

        #region constructors
        public csAttractionDataService(ILogger<csAttractionDataService> logger, csAttractionDbRepos repo ):this()
        {
            _logger = logger;
            _logger.LogInformation(_instanceHeartbeat); 

            _repo = repo;
        }

        #endregion
    }
}
