using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.DAL.Interfaces;

namespace WebSocketServer.Tests.Mocks
{
    public static class MockUowRepository
    {
        public static Mock<IDbRepos> GetMock()
        {
            var mock = new Mock<IDbRepos>();
            var MessageRepoMock = MockMessageRepository.GetMock();

            mock.Setup(m => m.Message).Returns(() => MessageRepoMock.Object);
            mock.Setup(m => m.Save()).Returns(() => { return 1; });
            return mock;
        }
    }
}
