using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


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
            listener.Start();
            //todo: уберите логику из конструктора, он должен выделять и инициаолизировать память, и не содержать лоигку
            while (listener.IsListening)
            {
                var context = listener.GetContext();
                //todo: замените последоватлеьность вызово на пару строчек, которые вытаскивают метод с помощью рефлексии и вызывают его
                if (context.Request.RawUrl.Contains("Ping"))
                {
                    Ping(context);
                }
                else if (context.Request.RawUrl.Contains("PostInputData"))
                {
                    PostInputData(context);
                }
                else if (context.Request.RawUrl.Contains("GetAnswer"))
                {
                    GetAnswer(context);
                }
                else if (context.Request.RawUrl.Contains("Stop"))
                {
                    Stop(context);
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
