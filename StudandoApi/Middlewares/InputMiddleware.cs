using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SudyApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class InputMiddleware
    {
        private readonly RequestDelegate _next;

        public InputMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Stopwatch timer = new Stopwatch();
            string responseBody = null;
            Stream originalBody = httpContext.Response.Body;

            try
            {
                string url = httpContext.Request.GetEncodedUrl().ToString();

                string requestBody = string.Empty;
                HttpRequest request = httpContext.Request;
                if (request.ContentLength > 0)
                {
                    HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);
                    byte[] bufferRequest = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(bufferRequest, 0, bufferRequest.Length);
                    requestBody = Encoding.UTF8.GetString(bufferRequest);
                    request.Body.Position = 0;
                }

                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    timer.Start();

                    await _next(httpContext);

                    timer.Stop();

                    memStream.Position = 0;
                    responseBody = new StreamReader(memStream).ReadToEnd();
                }

                //IMongoCollection<InputLogModel> collection = MongoDbUtility.GetCollection<InputLogModel>(MongoDbCollection.InputLog);

                //InputLogModel input = new InputLogModel(requestBody, responseBody, url, timer.Elapsed.Seconds, httpContext.Request.Method, httpContext.Response.StatusCode.ToString());

                //collection.InsertOne(input);

                var buffer = Encoding.UTF8.GetBytes(responseBody);

                using (var output = new MemoryStream(buffer))
                {
                    await output.CopyToAsync(originalBody);
                }
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }
        }
    }
}
