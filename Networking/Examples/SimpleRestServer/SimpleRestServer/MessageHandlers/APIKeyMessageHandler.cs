using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace SimpleRestServer.MessageHandlers
{
    public class APIKeyMessageHandler : DelegatingHandler
    {
        private const string CorrectAPIKey = "whencanifinishthisgame";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool isValideKey = false;
            IEnumerable<string> requestHeaders;
            var checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
            if(checkApiKeyExists && requestHeaders.FirstOrDefault().Equals(CorrectAPIKey))
            {
                isValideKey = true;
            }

            if (!isValideKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
            }

            //Calling base class Method
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}