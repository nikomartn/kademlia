using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration.Implementation
{
    public class Configuration : IConfiguration
    {
        protected Dictionary<string, string> data = new Dictionary<string, string>();
        public string this[string key] { get => data[key]; set => data[key] = value; }
    }
}
