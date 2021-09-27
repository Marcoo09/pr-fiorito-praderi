using System;
using Protocol;
using Server.Domain;

namespace Server.Interfaces
{
    public interface IServiceRouter
    {
        Frame GetResponse(Frame request, User user);
    }
}
