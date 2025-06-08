using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.Interfaces;
using NetworkUtility.Services;
using System.Net.NetworkInformation;

namespace NetworkUtility.Tests.PingTest
{
    public class NetworkServiceTests
    {
        private readonly IDnsService _dnsService;

        private readonly NetworkService _networkService;
        public NetworkServiceTests()
        {
            //Dependencies
            _dnsService = A.Fake<IDnsService>();

            //SUT
            _networkService = new NetworkService(_dnsService);
        }

        [Fact]
        public void NeworkService_SendPing_ReturnString()
        {
            A.CallTo(() => _dnsService.SendDns()).Returns(true);
            string result;

            result = _networkService.SendPing();

            result.Should().NotBeNullOrEmpty();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [InlineData(1,1,2)]
        [InlineData(2,2,4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int output)
        {
            int result;

            result = _networkService.PingTimeout(a, b);
            result.Should().Be(output);
        }

        [Fact]
        public void NetworkService_LastPingDate_ReturnDateTime()
        {
            //Arrange
            DateTime result;

            //Fact
            result = _networkService.LastPingDate();

            //Assert
            result.Should().BeAfter(1.February(2010));
            result.Should().BeBefore(2.March(2026));
        }

        [Fact]
        public void NetworkService_PingOptions_ReturnPingOptions()
        {
            //Arrange
            PingOptions pingOptions = new PingOptions(1,true);
            PingOptions result;

            //Fact
            result = _networkService.PingOptions();

            //Assert
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(pingOptions);
            result.Ttl.Should().Be(pingOptions.Ttl);
        }

        [Fact]
        public void NetworkService_Pings_ReturnIenumerablePingOptions()
        {
            //Arrange
            List<PingOptions> pingOptions = new List<PingOptions>()
            {
                new PingOptions(1,true),
                new PingOptions(2,true),
                new PingOptions(3,true),
                new PingOptions(4,false),
            };
            IEnumerable<PingOptions> result;

            //Fact
            result = _networkService.Pings();

            //Assert
            result.Should().BeOfType<List<PingOptions>>();
            result.Should().BeEquivalentTo(pingOptions);
            result.Should().Contain(x => x.DontFragment == false);
        }
    }
}
