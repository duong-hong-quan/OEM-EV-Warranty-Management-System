using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace BE.Services
{
    public class BaseService
    {
        private readonly IServiceProvider _serviceProvider;
        public BaseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public T? Resolve<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T))!;
        }



        public enum APIMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public async Task<RestResponse> CallAPIAsync(
          string endpoint,
          APIMethod method,
          object? data = null,
          string? bearerToken = null,
          Dictionary<string, string>? configHeaders = null)
        {
            var client = new RestClient();
            var request = new RestRequest(endpoint);

            // Thiết lập phương thức
            switch (method)
            {
                case APIMethod.GET:
                    request.Method = Method.Get;
                    break;
                case APIMethod.POST:
                    request.Method = Method.Post;
                    break;
                case APIMethod.PUT:
                    request.Method = Method.Put;
                    break;
                case APIMethod.DELETE:
                    request.Method = Method.Delete;
                    break;
            }

            // Thêm Bearer token nếu có
            if (!string.IsNullOrEmpty(bearerToken))
            {
                request.AddHeader("Authorization", $"Bearer {bearerToken}");
            }

            // Thêm headers tùy chỉnh nếu có
            if (configHeaders != null)
            {
                foreach (var header in configHeaders)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            // Thêm payload nếu phù hợp
            if (data != null &&
                (method == APIMethod.POST || method == APIMethod.PUT))
            {
                request.AddJsonBody(data);
            }

            // Gửi request
            var response = await client.ExecuteAsync(request);

            // Nếu status không thành công thì throw exception tương tự EnsureSuccessStatusCode()
            if (!response.IsSuccessful)
            {
                throw new HttpRequestException($"API call to {endpoint} failed with status {response.StatusCode}: {response.Content}");
            }

            return response;
        }
    }
}
