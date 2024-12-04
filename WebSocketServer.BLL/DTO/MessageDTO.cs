using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.DAL;

namespace WebSocketServer.BLL.DTO
{
    public class MessageDTO
    {
        public MessageDTO() { }
        public MessageDTO(Message message)
        {
            Id = message.Id;
            Content = message.Content;
            DateTime = message.DateTime != null && message.DateTime != "" ? System.DateTime.Parse(message.DateTime) : null;
            ConnectionId = message.ConnectionId;
        }

        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime? DateTime { get; set; }

        public string? ConnectionId { get; set; }
    }
}
