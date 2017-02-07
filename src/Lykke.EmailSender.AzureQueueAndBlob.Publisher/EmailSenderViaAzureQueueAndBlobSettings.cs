﻿using Common;
using Common.Log;
using Lykke.Integration.AzureQueueAndBlobs;
using Lykke.Integration.AzureQueueAndBlobs.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.EmailSender.AzureQueueAndBlob.Publisher
{
    public static class EmailSenderViaAzureQueueAndBlobBinding
    {
        public static EmailSenderViaQueue UseEmailSenderViaAzureQueueAndBlobPublisher(this IServiceCollection serviceCollection, 
            AzureQueueAndBlobIntegrationSettings settings, ILog log)
        {
            var applicationName =
                 Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationName;

            var publisher = new AzureQueuePublisher<EmailModel>(applicationName, settings)
                .SetLogger(log)
                .SetSerializer(new EmailSenderViaAzureQueueAndBlobSerializer())
                .Start();

            var emailSenderInstance = new EmailSenderViaQueue(publisher);

            serviceCollection.AddSingleton<IEmailSender>(emailSenderInstance);
            return emailSenderInstance;
        }

    }


    public class EmailSenderViaAzureQueueAndBlobSerializer : IAzureQueueSerializer<EmailModel>
    {
        public byte[] Serialize(EmailModel model)
        {
            return model.ToContract().ToJson().ToUtf8Bytes();
        }
    }
}
