using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DynamicsHelper.Dynamics;
using DynamicsHelper.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DynamicsHelper.Middlewares
{
    public class DynamicsApiProxyMiddleware
    {
        private readonly RequestDelegate _next;

        public DynamicsApiProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IDynamicsTokenService tokenService, IOptions<DynamicsSettings> dynamicsSetting)
        {
            await _next.Invoke(context);
            // TODO: Change on httpClientFactory

            var url =
                $"{dynamicsSetting.Value.Resource}{context.Request.Path.Value.Substring(1)}/{context.Request.QueryString.Value}";

            var token = await tokenService.CreateCreateAuthorizationHeader();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), url);
            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization",token);
            httpRequestMessage.Headers.TryAddWithoutValidation("Prefer",
               "odata.include-annotations=OData.Community.Display.V1.FormattedValue");


            // TODO: Client factory 
            HttpClient httpClient = new HttpClient();
            CopyFromOriginalRequestContentAndHeaders(context, httpRequestMessage);


            using (HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage))
            {
                CopyFromTargetResponseHeaders(context, responseMessage);
                    context.Response.StatusCode = (int)responseMessage.StatusCode;
                    if ((int) responseMessage.StatusCode != StatusCodes.Status204NoContent)
                    {
                        await responseMessage.Content.CopyToAsync(context.Response.Body);
                    }
            }
        }

        private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in context.Request.Headers)
            {
                if (header.Value.Contains("x-forwarded"))
                    break;

                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }
    }
}
