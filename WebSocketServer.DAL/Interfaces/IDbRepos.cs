using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.DAL.Interfaces
{
    public interface IDbRepos
    {
        IRepository<Message> Message {  get; }
        int Save();
    }
}
