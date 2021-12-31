using Configuration;
using System.Collections.Generic;

namespace KademliaTests.Dependencies
{
    internal class FakeConfiguration : IConfiguration
    {
        Dictionary<string, string> configurations = new Dictionary<string, string>();

        public string this[string key]
        {
            get => configurations[key];
            set => configurations[key] = value;
        }

        public void GenerateMocking()
        {
            this["Ip"] = "127.0.0.1";
            this["Port"] = "0";
            this["tRefresh"] = "9999999";
            this["tReplicate"] = "99999999";
            this["tRepublish"] = "99999999";
            this["K"] = "20";
        }
    }
}