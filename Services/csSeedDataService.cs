using System;
using DbContext;
using DbRepos;
using Microsoft.Extensions.Logging;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;

namespace Services
{
    public class csSeedDataService : ISeedDataService
    {        
        //Services injected
        private ILogger<csSeedDataService> _logger = null;
        private ISeedDbRepos _repo = null;


        #region only for layer verification
        private Guid _guid = Guid.NewGuid();
        private string _instanceHeartbeat;

        public string InstanceHeartbeat => _instanceHeartbeat;
        public csSeedDataService()
        {
            //only for layer verification
            _instanceHeartbeat = $"Heartbeat from class {this.GetType()} with instance Guid {_guid}. ";
        }
        #endregion

        public void MasterSeed() => _repo.MasterSeed();



        public csSeedDataService(ILogger<csSeedDataService> logger, ISeedDbRepos repo ):this()
        {
            _logger = logger;
            _logger.LogInformation(_instanceHeartbeat); 

            _repo = repo;
        }
        
    }
}
