using System.Net.NetworkInformation;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        public string SendPing()
        {
            return "Success: Ping Sent!";
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
