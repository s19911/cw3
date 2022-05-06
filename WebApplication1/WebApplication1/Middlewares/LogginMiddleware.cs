using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenie3.Middlewares
{
    public class LogginMiddleware
    {

 
        private readonly RequestDelegate _next;

        public LogginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request != null)
            {
                string sciezka = httpContext.Request.Path;
                string querystring = httpContext.Request?.QueryString.ToString();
                string metoda = httpContext.Request.Method.ToString();
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                //zapis do pliku


                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    Log(sciezka + "\n" + bodyStr, w);

                }


            }

            await _next(httpContext);
        }

        private void Log(string bodyStr, StreamWriter w)
        {

            w.Write("\n\n Entry:\n");
            w.Write(bodyStr);

            w.Flush();
            w.Close();

        }
    }
}