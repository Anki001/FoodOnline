using FoodOnline.Web.Common.Enums;
using FoodOnline.Web.Models;
using FoodOnline.Web.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace FoodOnline.Web.Services
{
    public abstract class BaseService : IBaseService
    {
        public ResponseDto Response { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            Response = new ResponseDto();
            HttpClientFactory = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = HttpClientFactory.CreateClient("FoodOnline");
                var httpMessage = new HttpRequestMessage();

                httpMessage.Headers.Add("Accept", "application/json");
                httpMessage.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data is not null)
                {
                    httpMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponse = null;

                switch (apiRequest.ApiType)
                {
                    case ApiType.POST:
                        httpMessage.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        httpMessage.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        httpMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpMessage.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(httpMessage);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var response = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(response);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
