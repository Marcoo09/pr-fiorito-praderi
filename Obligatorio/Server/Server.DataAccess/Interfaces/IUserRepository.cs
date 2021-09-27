using System.Collections.Generic;
using Server.Domain;

namespace Server.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        int Insert(User user);
        User Get(int id);
        List<User> GetAll();
        void Delete(int id);
    }
}
