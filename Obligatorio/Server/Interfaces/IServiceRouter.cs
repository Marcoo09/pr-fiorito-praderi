using System;
using Protocol;

namespace Server.Interfaces
{
    public interface IServiceRouter
    {
        Frame GetResponse(Frame request);
    }
}
