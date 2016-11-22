using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace riat_l3
{
    interface IServer
    {
        void Ping(HttpListenerContext textContext);
        void PostInputData(HttpListenerContext textContext);
        void GetAnswer(HttpListenerContext textContext);
        void Stop(HttpListenerContext textContext);
    }
}
