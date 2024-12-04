using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.BLL.DTO;

namespace WebSocketServer.BLL.Interfaces
{
    public interface IMessageService
    {
        List<MessageDTO> GetMessages();
        MessageDTO GetMessage(int id);
        void CreateMessage(MessageDTO message);
    }
}
