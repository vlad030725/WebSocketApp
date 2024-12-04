using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.DAL.Interfaces;
using WebSocketServer.DAL;

namespace WebSocketServer.Tests.Mocks
{
    public static class MockMessageRepository
    {
        public static List<Message> Messages = new List<Message>()
        {
            new Message() { Id = 1, Content = "qwerty", DateTime = DateTime.Now.ToString(), ConnectionId = "123" },
            new Message() { Id = 2, Content = "asdfgh", DateTime = DateTime.Now.ToString(), ConnectionId = "456" },
            new Message() { Id = 3, Content = "zxcvbn", DateTime = DateTime.Now.ToString(), ConnectionId = "789" },
        };

        public static Mock<IRepository<Message>> GetMock()
        {
            var mock = new Mock<IRepository<Message>>();

            mock.Setup(m => m.GetList()).Returns(() => Messages);
            mock.Setup(m => m.GetItem(It.IsAny<int>()))
                .Returns((long id) => Messages.FirstOrDefault(o => o.Id == id) ?? Messages[0]);
            mock.Setup(m => m.Create(It.IsAny<Message>()))
                .Callback(() => { return; });
            mock.Setup(m => m.Update(It.IsAny<Message>()))
               .Callback(() => { return; });
            mock.Setup(m => m.Delete(It.IsAny<int>()))
               .Callback(() => { return; });

            return mock;
        }
    }
}
