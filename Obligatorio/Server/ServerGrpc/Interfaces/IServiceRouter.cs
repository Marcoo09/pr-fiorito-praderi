using System;
using System.Threading.Tasks;
using Protocol;
using Server.Domain;

namespace Server.Interfaces
{
    public interface IServiceRouter
    {
        Task<Frame> GetResponseAsync(Frame request);
    }
}
