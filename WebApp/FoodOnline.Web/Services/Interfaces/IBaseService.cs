using FoodOnline.Web.Models;

namespace FoodOnline.Web.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        ResponseDto Response { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
