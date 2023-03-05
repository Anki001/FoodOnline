using System.Threading.Tasks;

namespace FoodOnline.MessageBus.Interfaces
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseMessage message, string topicName);
    }
}
