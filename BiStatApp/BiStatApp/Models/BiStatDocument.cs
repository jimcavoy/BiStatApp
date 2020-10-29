using System;
using System.Collections.Generic;
using System.Text;

namespace BiStatApp.Models
{
    public class BiStatDocument
    {
        public string Version { get; set; } = "0.1.0.1";
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
