using System.Text;
using Common;
using Lykke.Integration.AzureQueueAndBlobs.Publisher;
using Lykke.Integration.AzureQueueAndBlobs.Subscriber;

namespace Lykke.EmailSender.AzureQueueAndBlob.Publisher
{
    public class EmailSenderViaAzureQueueAndBlobSerializer : IAzureQueueAndBlobSerializer<EmailModel>
    {
        public byte[] Serialize(EmailModel model)
        {
            return model.ToContract().ToJson().ToUtf8Bytes();
        }
    }

    public class EmailAzureQueueAndBlobDeserializer : IAzureQueueAndBlobDeserializer<EmailModel>
    {
        public EmailModel Deserialize(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            var contract = json.DeserializeJson<EmailSenderViaQueueContract>();

            return contract.ToDomain();
        }
    }
}
