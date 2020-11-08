using System;
using System.Collections.Generic;
using System.Text;

namespace bottlelib
{
    public class Player
    {
        public string PlayerId { get; set; }
        public string Url { get; set; }
        public string History { get; set; }

        public long InnerId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Profile { get; set; }
        public string Status { get; set; }
        public string AuthResult { get; set; }

        public int Balance { get; set; }
        public bool IsBonus { get; set; }
        public int Bonus { get; set; }

        public string Token { get; set; }

        //public string Token2 { get; set; }

        public bool IsSelected { get; set; }

    }
}
