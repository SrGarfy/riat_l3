using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Reflection;


namespace riat_l3
{
    class Server : IServer
    {
        private string port;
        private readonly HttpListener listener = new HttpListener();
        private Input input = null;
        private Output output = null;


        public Server(string port)
        {
            this.port = port;
            listener.Prefixes.Add($"http://127.0.0.1:{port}/Ping/");
            listener.Prefixes.Add($"http://127.0.0.1:{port}/GetAnswer/");
            listener.Prefixes.Add($"http://127.0.0.1:{port}/PostInputData/");
            listener.Prefixes.Add($"http://127.0.0.1:{port}/Stop/");
        }

        public void Start()
        {
            listener.Start();
            while (listener.IsListening)
            {
                var context = listener.GetContext();
                var url = context.Request.RawUrl;
                var method = GetType().GetMethods().FirstOrDefault(x => url.Contains(x.Name));
                if (method != null)
                {
                    method.Invoke(this, new object[] {context});
                }
            }
        }

        public void Ping(HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var response = context.Response.OutputStream;
            var outputStream = new StreamWriter(response);
            outputStream.Write("");
            outputStream.Close();
        }
        public void PostInputData(HttpListenerContext context)
        {
            var request = context.Request.InputStream;
            var inputStream = new StreamReader(request);
            byte[] inputObject = Encoding.UTF8.GetBytes(inputStream.ReadToEnd());
            input = JsonSerializer.Deserialize<Input>(inputObject);
            inputStream.Close();
            var response = context.Response.OutputStream;
            var outputStream = new StreamWriter(response);
            outputStream.Write("");
            outputStream.Close();
        }
        public void GetAnswer(HttpListenerContext context)
        {
            output = input.CreateOutput();
            var data = JsonSerializer.Serialize(output);
            var response = context.Response;
            response.ContentLength64 = data.Length;
            var outputStream = response.OutputStream;
            outputStream.Write(data, 0, data.Length);
            outputStream.Close();
        }
        public void Stop(HttpListenerContext context)
        {
            var response = context.Response.OutputStream;
            var outputStream = new StreamWriter(response);
            outputStream.Write("");
            listener.Stop();
        }

    }
}
