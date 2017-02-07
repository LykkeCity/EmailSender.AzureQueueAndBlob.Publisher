using System;
using System.Linq;
using Common;

namespace Lykke.EmailSender.AzureQueueAndBlob.Publisher
{
    public class EmailSenderViaQueueContract
    {

        public class EmailAttachmentContract
        {
            public string FileName { get; set; }
            public string Mime { get; set; }
            public string Data { get; set; }

        }

        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        public EmailAttachmentContract[] Attachments { get; set; }
    }


    public static class EmailSenderViaQueueContractMethodExts
    {

        public static EmailSenderViaQueueContract ToContract(this EmailModel src)
        {
            return new EmailSenderViaQueueContract
            {
                Email = src.Email,
                Body = src.Body,
                Subject = src.Subject,
                IsHtml = src.IsHtml,
                Attachments = src.Attachments?.Select(itm => itm.ToContract()).ToArray()
            };
        }


        public static EmailSenderViaQueueContract.EmailAttachmentContract ToContract(this EmailAttachment src)
        {
            return new EmailSenderViaQueueContract.EmailAttachmentContract
            {
                Mime = src.Mime,
                FileName = src.FileName,
                Data = src.Data.ToBase64()
            };
        }


        public static EmailModel ToDomain(this EmailSenderViaQueueContract src)
        {
            return new EmailModel
            {
                Body = src.Body,
                Subject = src.Subject,
                Email = src.Email,
                Attachments = src.Attachments?.Select(itm => itm.ToDomain()).ToArray()
            };
        }

        public static EmailAttachment ToDomain(this EmailSenderViaQueueContract.EmailAttachmentContract src)
        {
            return new EmailAttachment
            {
                FileName = src.FileName,
                Mime = src.Mime,
                Data = Convert.FromBase64String(src.Data) 
            };
        }

    }
}

