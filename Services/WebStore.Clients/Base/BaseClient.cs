using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly HttpClient _Client;

        protected readonly string _ServiceAddress;

        protected BaseClient(IConfiguration config, string ServiceAddress)
        {
            _ServiceAddress = ServiceAddress;

            _Client = new HttpClient
            {
                BaseAddress = new Uri(config["WebApiURL"])
            };

            var headers = _Client.DefaultRequestHeaders.Accept;
            headers.Clear();
            headers.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed || !disposing) return;
            _Disposed = true;
            _Client.Dispose();
        } 

        #endregion
    }
}
