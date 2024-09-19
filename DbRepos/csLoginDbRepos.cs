﻿using Configuration;
using Models;
using Models.DTO;
using DbModels;
using DbContext;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace DbRepos
{
    public class CsLoginDbRepos
    {
        #region soon to be used, commented out to reduce warnings
        private ILogger<CsLoginDbRepos> _logger = null;
        private string _dbseed = "sysadmin"; 
        private string _dblogin = "gstusr";
        #endregion

        #region contructors
        public CsLoginDbRepos() { }
        /*
        public csLoginDbRepos(ILogger<csLoginDbRepos> logger)
        {
            _logger = logger;
        }
        */
        #endregion

        public async Task<usrInfoDto> SeedAsync(int nrOfUsers, int nrOfSuperUsers)
        {
            using (var db = csMainDbContext.DbContext(_dbseed))
        {
            throw new NotImplementedException();        
        }
        }


        public async Task<loginUserSessionDto> LoginUserAsync(loginCredentialsDto usrCreds)
        {
            using (var db = csMainDbContext.DbContext(_dblogin))
            using (var cmd1 = db.Database.GetDbConnection().CreateCommand())
            {
                throw new NotImplementedException();        
            }
        }


        //Industry strength encryption of passwords
        //Notice how I read the Salt and the Iterations from csAppConfig, which is stored in user secrets
        private static string EncryptPassword(string _password)
        {
            //Hash a password using salt and streching
            byte[] encrypted = KeyDerivation.Pbkdf2(
                password: _password,
                salt: Encoding.UTF8.GetBytes(csAppConfig.PasswordSalt.Salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: csAppConfig.PasswordSalt.Iterations,
                numBytesRequested: 64);

            return Convert.ToBase64String(encrypted);
        }
    }
}

