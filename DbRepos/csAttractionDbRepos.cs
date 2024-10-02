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
using System.Linq.Expressions;
//Author: Torsten Ek


namespace DbRepos;

public class csAttractionDbRepos
{
    private ILogger<csAttractionDbRepos> _logger = null;
    public string Hearstring {get;} = $"Heartbeat from namespace {nameof(DbRepos)}, class {nameof(csAttractionDbRepos)}";

    #region seeding
    //creates master seed
    public async Task<seedInfoDto> CreateSeedAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {

            //Making sure database is cleared of seeded data
            await RemoveSeedAsync(true);

            //Seeding lists
            var _seeder = new csSeedGenerator();
            var _localities = _seeder.UniqueItemsToList<csLocalityDbM>(_seeder.Next(100, 200));
            var _users = _seeder.UniqueItemsToList<csUserDbM>(_seeder.Next(300, 500));
            var _attractions = _seeder.UniqueItemsToList<csAttractionDbM>(_seeder.Next(1000 ,2000));

            //Adding comments and localities to attractions
            _attractions.ForEach(a => a.CommentsDbM = _seeder.UniqueItemsToList<csCommentDbM>(_seeder.Next(0,21)));
            _attractions.ForEach(a => a.LocalityDbM = _localities[_seeder.Next(0, _localities.Count)] );

            
            //Assigning users to the comments
            _attractions.ForEach(a => a.CommentsDbM.ForEach(c => c.UserDbM = _users[_seeder.Next(0, _users.Count)]));

            //Adding data to db
            db.Users.AddRange(_users);
            db.Localities.AddRange(_localities);
            db.Attractions.AddRange(_attractions);

            var _info = new seedInfoDto()
            {
                nrSeededAttractions = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csAttractionDbM) && entry.State == EntityState.Added),

                nrSeededComments = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csCommentDbM) && entry.State == EntityState.Added),

                nrSeededUsers = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csUserDbM) && entry.State == EntityState.Added),

                nrSeededLocality = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csLocalityDbM) && entry.State == EntityState.Added)
                
            };
            try 
            {

                await db.SaveChangesAsync();
                _logger.LogInformation("Seeded");
            }
            catch(Exception e)
            {
                _logger.LogInformation("Failed to seed");
                _logger.LogInformation(e.ToString());
            }
            return _info;


        }  
    }

    //deletes master seed
    public async Task<deleteSeedInfoDto> RemoveSeedAsync(bool seeded)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {

            db.Attractions.RemoveRange(db.Attractions.Where(a => a.Seeded == seeded ));
            db.Users.RemoveRange(db.Users.Where(a => a.Seeded == seeded ));
            db.Localities.RemoveRange(db.Localities.Where(a => a.Seeded == seeded ));
            db.Comments.RemoveRange(db.Comments.Where(a => a.Seeded == seeded ));
            var _info = new deleteSeedInfoDto()
            {
                nrDeletedAttractions = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csAttractionDbM) && entry.State == EntityState.Deleted),

                nrDeletedComments = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csCommentDbM) && entry.State == EntityState.Deleted),

                nrDeletedUsers = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csUserDbM) && entry.State == EntityState.Deleted),

                nrDeletedLocality = db.ChangeTracker.Entries().Count(entry => 
                (entry.Entity is csLocalityDbM) && entry.State == EntityState.Deleted)
                
            };

            try 
            { 
                await db.SaveChangesAsync(); 
                string response = seeded ? "Seeded" : "Unseeded";
                _logger.LogInformation($"Removed {response} data");
            }
            catch(Exception e)
            {
                _logger.LogInformation("Failed to remove Seeded data");
                _logger.LogInformation(e.ToString());
   
            }
            return _info;
        }
    }

    #endregion

    #region reading
    public async Task<csRespPageDTO<IAttraction>> ReadAllAttractionsAsync(bool seeded, bool flat, int pageNumber, int pageSize, string filter)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            filter ??= "";
            IQueryable<csAttractionDbM> _query;
            if (flat)
            {
                _query = db.Attractions.AsNoTracking();
            }
            else
            {
                _query = db.Attractions
                .Include(a => a.LocalityDbM)
                .AsNoTracking();
            }
            
            /*
            because I get error from efc saying i can't do ToString on an enum and the error saying i should implement .AsEnumarble
            I implement .AsEnumerable and it works.
            */
            
            
            var _count = await _query
            .AsEnumerable()
            .Where(a => a.Seeded == seeded && 
                        (   
                            a.Category.ToString().ToLower().Contains(filter) ||
                            a.Description.ToLower().Contains(filter) ||
                            a.Name.ToLower().Contains(filter)||
                            a.LocalityDbM.Country.ToLower().Contains(filter) ||
                            a.LocalityDbM.City.ToLower().Contains(filter)||
                            a.LocalityDbM.StreetAddress.ToLower().Contains(filter)
                        )
                    )
            .CountAsync();

            
            var _attractions = await _query

            .Where(a => a.Seeded == seeded && 
                        (a.Category.ToString().ToLower().Contains(filter) ||
                        a.Description.ToLower().Contains(filter) ||
                        a.Name.ToLower().Contains(filter) ||
                        a.LocalityDbM.Country.ToLower().Contains(filter) ||
                        a.LocalityDbM.City.ToLower().Contains(filter)||
                        a.LocalityDbM.StreetAddress.ToLower().Contains(filter))
                    )
    
            
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
            //_attractions.ForEach(u => u.LocalityDbM?.ExcludeNavProps());

            var _info = new csRespPageDTO<IAttraction>()
            {
                
                DbItemsCount = _count,

                PageItems =  _attractions.ToList<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize

                
            };
            _logger.LogInformation("executed ReadAllAttractionsAsync()");
            return _info;
        }
    }
    public async Task<csRespPageDTO<IUser>> ReadAllUsersAsync(bool seeded, bool flat, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            
            IQueryable<csUserDbM> _query;
            if (flat)
            {
                _query = db.Users.AsNoTracking();
            }
            else
            {
                _query = db.Users
                .Include(u => u.CommentsDbM)
                .AsNoTracking();
            }
            var _count = await _query
                    .Where(u => u.Seeded == seeded)
                    .CountAsync();
            var _users = await _query
                .Where(u => u.Seeded == seeded)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
            .ToListAsync();
            
            _users.ForEach(u => u.CommentsDbM?.ForEach( c => c.ExcludeNavProps()));

            var _info = new csRespPageDTO<IUser>()
            {
                
                DbItemsCount = _count,

                PageItems =  _users.ToList<IUser>(),

                PageNr = pageNumber,
                PageSize = pageSize

                
            };
            
            _logger.LogInformation("executed ReadAllUserAsync()");
            return _info;



        }
    }
    public async Task<IAttraction> ReadAttractionAsync(bool flat, Guid _attractionId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            
            IQueryable<csAttractionDbM> _query;
            if (flat)
            {
                _query = db.Attractions
                .AsNoTracking()
                .Where(a => a.AttractionId == _attractionId)
                .Include(a => a.LocalityDbM);
            }
            else
            {
                _query = db.Attractions
                .AsNoTracking()
                .Where(a => a.AttractionId == _attractionId)
                .Include(a => a.CommentsDbM)
                .Include(a => a.LocalityDbM);
            }
            
            var _attractions = await _query.ToListAsync();
            _attractions.ForEach(a => a.CommentsDbM?.ForEach(c => c.ExcludeNavProps()));  
            _attractions.ForEach(a => a.LocalityDbM?.ExcludeNavProps());     
                     

            _logger.LogInformation("executed ReadAttractionAsync()");
            return  _attractions.FirstOrDefault<IAttraction>();
            
        }
    }
    public async Task<IUser> ReadUserAsync(bool flat, Guid _userId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            
            IQueryable<csUserDbM> _query;
            if (flat)
            {
                _query = db.Users.AsNoTracking()
                .Where(u => u.UserId == _userId);
            }
            else
            {
                _query = db.Users
                .Include(u => u.CommentsDbM)
                .Where(u => u.UserId == _userId)
                .AsNoTracking();
            }
            
            var _users = await _query.ToListAsync();
            _users.ForEach(a => a.CommentsDbM?.ForEach(c => c.ExcludeNavProps2()));       
                     

            _logger.LogInformation("executed ReadUserAsync()");
            return  _users.FirstOrDefault<IUser>();
            
        }
    }

    public async Task<IComment> ReadCommentAsync(bool flat, Guid _commentId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            
            IQueryable<csCommentDbM> _query;
            if (flat)
            {
                _query = db.Comments.AsNoTracking()
                .Where(c => c.CommentId == _commentId);
            }
            else
            {
                _query = db.Comments.AsNoTracking()
                .Where(c => c.CommentId == _commentId)
                .Include(c => c.UserDbM)
                .Include(c => c.AttractionDbM);
            }
            
            var _users = await _query.ToListAsync();
            //_users.ForEach(a => a.AttractionDbM?.ExcludeNavProps());      
            //_users.ForEach(a => a.UserDbM?.ExcludeNavProps());  
                     

            _logger.LogInformation("executed ReadCommentAsync()");
            return  _users.FirstOrDefault<IComment>();
            
        }
    }

    
    public async Task<ILocality> ReadLocalityAsync(bool flat, Guid _localityId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            
            IQueryable<csLocalityDbM> _query;
            if (flat)
            {
                _query = db.Localities.AsNoTracking()
                .Where(c => c.LocalityId == _localityId);
            }
            else
            {
                _query = db.Localities.AsNoTracking()
                .Where(c => c.LocalityId == _localityId)
                .Include(c => c.AttractionsDbM);
            }
            
            var _localities = await _query.ToListAsync();
            //_users.ForEach(a => a.AttractionDbM?.ExcludeNavProps());      
            //_users.ForEach(a => a.UserDbM?.ExcludeNavProps());  
                     

            _logger.LogInformation("executed ReadLocalityAsync()");
            return  _localities.FirstOrDefault<ILocality>();
            
        }
    }
    //Reads all attractions with no comments
    public async Task<csRespPageDTO<IAttraction>> ReadAllNoCommentsAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            IQueryable<csAttractionDbM> _query = db.Attractions
                .Include(a => a.CommentsDbM)
                .Include(a => a.LocalityDbM)
                .AsNoTracking();

            var _info = new csRespPageDTO<IAttraction>(){
                PageItems = await _query.ToListAsync<IAttraction>(),
                DbItemsCount = await _query.Where( a => !a.CommentsDbM.Any() ).CountAsync(),
            };
            _logger.LogInformation("executed ReadAllNoCommentsAsync");
            return _info;
        }
    }
    
    #endregion

    #region deleting
    
    public async Task<IUser> DeleteUserAsync(Guid userId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Users
                .Where(u => u.UserId == userId);
            var item = await _query.FirstOrDefaultAsync<csUserDbM>();

            if (item == null) throw new ArgumentException($"User with id {userId} not existing");
            db.Users.Remove(item);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Deleted item with id {userId}");
            return item;
        }
        
    }

    public async Task<IAttraction> DeleteAttractionAsync(Guid attractionId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Attractions
                .Where(a => a.AttractionId == attractionId);
            var item = await _query.FirstOrDefaultAsync<csAttractionDbM>();

            if (item == null) throw new ArgumentException($"Attraction with id {attractionId} not existing");
            db.Attractions.Remove(item);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Deleted item with id {attractionId}");
            return item;
        }
        
    }
    public async Task<IComment> DeleteCommentAsync(Guid commentId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Comments
                .Where(c => c.CommentId == commentId);
            var item = await _query.FirstOrDefaultAsync<csCommentDbM>();

            if (item == null) throw new ArgumentException($"Comment with id {commentId} not existing");
            db.Comments.Remove(item);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Deleted item with id {commentId}");
            return item;
        }
        
    }

    public async Task<ILocality> DeleteLocalityAsync(Guid localityId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Localities
                .Where(c => c.LocalityId == localityId);
            var item = await _query.FirstOrDefaultAsync<csLocalityDbM>();

            if (item == null) throw new ArgumentException($"Comment with id {localityId} not existing");
            db.Localities.Remove(item);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Deleted item with id {localityId}");
            return item;
        }
        
    }
    #endregion

    #region updating
    public async Task<ILocality> UpdateLocality(csLocalityCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Localities
                .Where(a => a.LocalityId == itemDto.LocalityId);

            var item = await _query
                .Include(a => a.AttractionsDbM)
                .FirstOrDefaultAsync<csLocalityDbM>();

            if (item == null) throw new ArgumentException($"Locality with id {itemDto.LocalityId} not existing");

            item.UpdateFromDTO(itemDto);

            await navProp_csLocalityCUdto_to_csLocalityDbm(db, itemDto, item);

            db.Localities.Update(item);

            await db.SaveChangesAsync();

            return await ReadLocalityAsync(false, item.LocalityId);
        }
        
    }
    public async Task<IAttraction> UpdateAttraction(csAttractionCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Attractions
                .Where(a => a.AttractionId == itemDto.AttractionId);
            var item = await _query
                .Include(a => a.CommentsDbM)
                .Include(a => a.LocalityDbM)
                .FirstOrDefaultAsync<csAttractionDbM>();

            if (item == null) throw new ArgumentException($"Attraction with id {itemDto.AttractionId} not existing");

            item.UpdateFromDTO(itemDto);

            await navProp_csAttractionCUdto_to_csAttractionDbm(db, itemDto, item);

            db.Attractions.Update(item);

            await db.SaveChangesAsync();

            return await ReadAttractionAsync(false, item.AttractionId);
        }
        
    } 
    public async Task<IUser> UpdateUser(csUserCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Users
                .Where(a => a.UserId == itemDto.UserId);

            var item = await _query
                .Include(a => a.CommentsDbM)
                .FirstOrDefaultAsync<csUserDbM>();

            if (item == null) throw new ArgumentException($"User with id {itemDto.UserId} not existing");

            item.UpdateFromDTO(itemDto);

            await navProp_csUserCUdto_to_csUserDbM(db, itemDto, item);

            db.Users.Update(item);

            await db.SaveChangesAsync();

            return await ReadUserAsync(false, item.UserId);
        }
        
    }
    public async Task<IComment> UpdateComment(csCommentCUdto itemDto)
    {
        using (var db = csMainDbContext.DbContext("sysadmin")) 
        {
            var _query = db.Comments
                .Where(a => a.CommentId == itemDto.CommentId);

            var item = await _query
                .Include(a => a.AttractionDbM)
                .Include(a => a.UserDbM)
                .FirstOrDefaultAsync<csCommentDbM>();

            if (item == null) throw new ArgumentException($"Comment with id {itemDto.CommentId} not existing");

            item.UpdateFromDTO(itemDto);

            await navProp_csCommentCUdto_to_csComentDbM(db, itemDto, item);

            db.Comments.Update(item);

            await db.SaveChangesAsync();

            return await ReadCommentAsync(false, item.CommentId);
        }
        
    }
   
    private static async Task navProp_csLocalityCUdto_to_csLocalityDbm(csMainDbContext db, csLocalityCUdto _itemDtoSrc, csLocalityDbM _itemDst)
    {


        //update AttractionDbM from itemDto.AttractionId
        List<csAttractionDbM> _attractions = null;
        if(_itemDtoSrc.AttractionsId != null)
        {
            _attractions = new List<csAttractionDbM>(); 
            foreach (var id in _itemDtoSrc.AttractionsId)
            {
                var c = await db.Attractions.FirstOrDefaultAsync(c => c.AttractionId == id);
                if (c == null) throw new ArgumentNullException($"Attraction id {id} not existing");
                _attractions.Add(c);
            }
        }
        _itemDst.AttractionsDbM = _attractions;


    }
    private static async Task navProp_csAttractionCUdto_to_csAttractionDbm(csMainDbContext db, csAttractionCUdto _itemDtoSrc, csAttractionDbM _itemDst)
    {

        //update LocalityDbm from itemDto.LocalityId 
        _itemDst.LocalityDbM = (_itemDst != null) ?  await db.Localities.FirstOrDefaultAsync(
            l => (_itemDtoSrc.LocalityId == l.LocalityId)) : null;


        //update CommentsDbm from itemDto.CommentsId
        List<csCommentDbM> _comments = null;
        if(_itemDtoSrc.CommentsId != null)
        {
            _comments = new List<csCommentDbM>(); 
            foreach (var id in _itemDtoSrc.CommentsId)
            {
                var c = await db.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
                if (c == null) throw new ArgumentNullException($"Comment id {id} not existing");
                _comments.Add(c);
            }
        }
        _itemDst.CommentsDbM = _comments;


    }
    private static async Task navProp_csUserCUdto_to_csUserDbM(csMainDbContext db, csUserCUdto _itemDtoSrc, csUserDbM _itemDst)
    {

        //update CommentsDbm from itemDto.CommentsId
        List<csCommentDbM> _comments = null;
        if(_itemDtoSrc.CommentsId != null)
        {
            _comments = new List<csCommentDbM>(); 
            foreach (var id in _itemDtoSrc.CommentsId)
            {
                var c = await db.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
                if (c == null) throw new ArgumentNullException($"Comment id {id} not existing");
                _comments.Add(c);
            }
        }
        _itemDst.CommentsDbM = _comments;


    }
    
    private static async Task navProp_csCommentCUdto_to_csComentDbM(csMainDbContext db, csCommentCUdto _itemDtoSrc, csCommentDbM _itemDst)
    {

        //update navigation UserDbm from itemDto.UserDbm 
        _itemDst.UserDbM = (_itemDst != null) ?  await db.Users.FirstOrDefaultAsync(
            u => (_itemDtoSrc.UserId == u.UserId)) : null;

        
        //update navigation AttractionDbm from itemDto.AttractionDbm 
        _itemDst.AttractionDbM = (_itemDst != null) ?  await db.Attractions.FirstOrDefaultAsync(
            u => (_itemDtoSrc.AttractionId == u.AttractionId)) : null;

    }
    #endregion
   
    #region creating

    public async Task<IAttraction> CreateAttractionAsync(csAttractionCUdto itemDto)
    {
        if(itemDto.AttractionId != null) 
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var item = new csAttractionDbM(itemDto);

            await navProp_csAttractionCUdto_to_csAttractionDbm(db, itemDto, item);

            db.Attractions.Add(item);

            await db.SaveChangesAsync();

            return await ReadAttractionAsync(false, item.AttractionId);
        }

    }
    public async Task<IUser> CreateUserAsync(csUserCUdto itemDto)
    {
        if(itemDto.UserId != null) 
            throw new ArgumentException($"{nameof(itemDto.UserId)} must be null when creating a new object");
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var item = new csUserDbM(itemDto);

            await navProp_csUserCUdto_to_csUserDbM(db, itemDto, item);

            db.Users.Add(item);

            await db.SaveChangesAsync();

            return await ReadUserAsync(false, item.UserId);
        }

    }

    public async Task<IComment> CreateCommentAsync(csCommentCUdto itemDto)
    {
        if(itemDto.CommentId != null) 
            throw new ArgumentException($"{nameof(itemDto.CommentId)} must be null when creating a new object");
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var item = new csCommentDbM(itemDto);

            await navProp_csCommentCUdto_to_csComentDbM(db, itemDto, item);

            db.Comments.Add(item);
            

            await db.SaveChangesAsync();

            return await ReadCommentAsync(false, item.CommentId);
        }

    }
    public async Task<ILocality> CreateLocalityAsync(csLocalityCUdto itemDto)
    {
        if(itemDto.LocalityId != null) 
            throw new ArgumentException($"{nameof(itemDto.LocalityId)} must be null when creating a new object");
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var item = new csLocalityDbM(itemDto);

            await navProp_csLocalityCUdto_to_csLocalityDbm(db, itemDto, item);

            db.Localities.Add(item);
            

            await db.SaveChangesAsync();

            return await ReadLocalityAsync(false, item.LocalityId);
        }

    }

    #endregion
    public csAttractionDbRepos(ILogger<csAttractionDbRepos> logger)
    {
        _logger = logger;
        _logger.LogInformation(Hearstring);

    }


}
