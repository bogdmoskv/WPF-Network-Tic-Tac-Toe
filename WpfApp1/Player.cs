using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Player
    {
        public string Name { get; set; }
        public Socket Socket { get; set; } 
        public int Count { get; set; }
    }
}
