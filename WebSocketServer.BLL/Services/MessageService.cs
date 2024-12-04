using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.BLL.DTO;
using WebSocketServer.BLL.Interfaces;
using WebSocketServer.DAL.Interfaces;
using WebSocketServer.DAL;

namespace WebSocketServer.BLL.Services
{
    public class MessageService : IMessageService
    {
        private IDbRepos db;

        public MessageService(IDbRepos db)
        {
            this.db = db;
        }

        public void CreateMessage(MessageDTO message)
        {
            db.Message.Create(new Message()
            {
                Content = message.Content,
                DateTime = DateTime.Now.ToString(),
                ConnectionId = message.ConnectionId,
            });
            SaveChanges();
        }

        public MessageDTO GetMessage(int id)
        {
            return new MessageDTO(db.Message.GetItem(id));
        }

        public List<MessageDTO> GetMessages()
        {
            return db.Message.GetList().Select(i => new MessageDTO(i)).ToList();
        }

        public bool SaveChanges()
        {
            if (db.Save() > 0) return true;
            return false;
        }
    }
}
