using Configuration;
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
using Seido.Utilities.SeedGenerator;
//Author: Torsten Ek


namespace DbRepos;

public class csSeedDbRepos : ISeedDbRepos
{
    private ILogger<csSeedDbRepos> _logger = null;
    public string Hearstring {get;} = $"Heartbeat from namespace {nameof(DbRepos)}, class {nameof(csSeedDbRepos)}";
    public void MasterSeed()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {

            //Seeding data
            var _seeder = new csSeedGenerator();
            List<csLocalityDbM> _localities = _seeder.UniqueItemsToList<csLocalityDbM>(100);
            List<csUserDbM> _users = _seeder.UniqueItemsToList<csUserDbM>(50);
            List<csAttractionDbM> _attractions = _seeder.UniqueItemsToList<csAttractionDbM>(1000);

            _attractions.ForEach(a => a.comments =
                                        _seeder.UniqueItemsToList<csCommentDbM>(_seeder.Next(0,21))
                                        .ToList<IComment>()
                                );

            //sätt in localities i attractions, lägg till fler efc annotations i dbmodels
            //fortsätta i cslocalitydbm, kolla relationen testa modellen
            //_attractions.ForEach(x => System.Console.WriteLine(x));

            

            //Adding data to db
            
            db.Users.AddRange(_users);
            db.Attractions.AddRange(_attractions);
            db.Localities.AddRange(_localities);
            db.SaveChanges();

            _logger.LogInformation("Seeded master seed");
        }  
    }
    public csSeedDbRepos(ILogger<csSeedDbRepos> logger)
    {
        _logger = logger;
        _logger.LogInformation(Hearstring);

    }


}
