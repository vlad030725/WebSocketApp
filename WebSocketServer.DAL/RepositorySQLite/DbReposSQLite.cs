using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.DAL.Interfaces;

namespace WebSocketServer.DAL.RepositorySQLite
{
    public class DbReposSQLite : IDbRepos
    {
        private WebSocketContext db;
        private MessageRepositorySQLite MessageRepository;

        public DbReposSQLite()
        {
            db = new WebSocketContext();
        }

        public IRepository<Message> Message
        {
            get
            {
                if (MessageRepository == null)
                    MessageRepository = new MessageRepositorySQLite(db);
                return MessageRepository;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
