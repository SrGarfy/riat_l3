using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace riat_l3
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = Console.ReadLine();
            var server = new Server(port);
        }
    }
}
