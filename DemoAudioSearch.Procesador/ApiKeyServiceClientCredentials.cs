using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoAudioSearch.Procesador
{
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["ApiKeyTextService"]);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
