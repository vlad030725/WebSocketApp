using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.DAL.Interfaces;

namespace WebSocketServer.DAL.RepositorySQLite
{
    public class MessageRepositorySQLite : IRepository<Message>
    {
        private WebSocketContext db;

        public MessageRepositorySQLite(WebSocketContext db)
        {
            this.db = db;
        }

        public void Create(Message item)
        {
            db.Messages.Add(item);
        }

        public void Delete(int id)
        {
            Message? item = db.Messages.Find(id);
            if (item != null)
            {
                db.Messages.Remove(item);
            }
        }

        public Message? GetItem(int id)
        {
            return db.Messages.Find(id);
        }

        public List<Message> GetList()
        {
            return db.Messages.ToList();
        }

        public void Update(Message item)
        {
            db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

    }
}
