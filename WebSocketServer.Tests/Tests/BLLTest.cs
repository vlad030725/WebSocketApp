using Moq;
using WebSocketServer.BLL.Interfaces;
using WebSocketServer.BLL.DTO;
using WebSocketServer.BLL.Services;
using WebSocketServer.DAL.Interfaces;
using WebSocketServer.Tests.Mocks;

namespace WebSocketServer.Tests.Tests
{
    public class BLLTest
    {
        private readonly Mock<IDbRepos> uowMock;
        private readonly IMessageService service;

        private readonly List<MessageDTO> ValidListMessages = new List<MessageDTO>()
        {
            new MessageDTO() { Id = 1, Content = "qwerty", DateTime = DateTime.Now, ConnectionId = "123" },
            new MessageDTO() { Id = 2, Content = "asdfgh", DateTime = DateTime.Now, ConnectionId = "456" },
            new MessageDTO() { Id = 3, Content = "zxcvbn", DateTime = DateTime.Now, ConnectionId = "789" },
        };

        public BLLTest()
        {
            uowMock = MockUowRepository.GetMock();
            service = new MessageService(uowMock.Object);
        }

        [Fact]
        public void GetListMessages_Success()
        {
            bool result = true;
            var messages = service.GetMessages();

            if (messages.Count == ValidListMessages.Count)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    if (messages[i].Id != ValidListMessages[i].Id ||
                        messages[i].Content != ValidListMessages[i].Content ||
                        messages[i].ConnectionId != ValidListMessages[i].ConnectionId)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            Assert.True(result);
        }

        [Fact]
        public void GetMessageById_Success()
        {
            var message = service.GetMessage(1);

            Assert.NotNull(message);
            Assert.Equal(message.Id, 1);
        }
    }
}