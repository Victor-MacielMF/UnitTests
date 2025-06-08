using NetworkUtility.Interfaces;
using System.Net.NetworkInformation;

namespace NetworkUtility.Services
{
    public class NetworkService
    {
        private readonly IDnsService _dnsService;
        public NetworkService(IDnsService dnsService)
        {
            _dnsService = dnsService;
        }

        public string SendPing()
        {
            bool result = _dnsService.SendDns();
            if (result)
            {
                return "Success: Ping Sent!";

            }
            else
            {
                return "Failled: Ping Unsent!";
            }
        }

        public int PingTimeout(int a, int b)
        {
            return a + b;
        }

        public DateTime LastPingDate()
        {
            return DateTime.Now;
        }

        public PingOptions PingOptions()
        {
            return new PingOptions( 1, true );
        }

        public IEnumerable<PingOptions> Pings()
        {
            return new List<PingOptions>()
            {
                new PingOptions( 1, true ),
                new PingOptions( 2, true ),
                new PingOptions( 3, true ),
                new PingOptions( 4, false ),
            };
        }
    }
}
