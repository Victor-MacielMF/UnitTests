using FluentAssertions;
using NetworkUtility.Ping;

namespace NetworkUtility.Tests.PingTest
{
    public class NetworkServiceTests
    {
        [Fact]
        public void NeworkService_SendPing_ReturnString()
        {
            NetworkService pingService = new NetworkService();
            string result;

            result = pingService.SendPing();

            result.Should().NotBeNullOrEmpty();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [InlineData(1,1,2)]
        [InlineData(2,2,4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int output)
        {
            NetworkService pingService = new NetworkService();
            int result;

            result = pingService.PingTimeout(a, b);
            result.Should().Be(output);
        }
    }
}
