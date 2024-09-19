using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public interface ISeedDbRepos {
    //public void Seed(int _count);

    public void MasterSeed();

}