using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class ReadGroupMessage
    {
        public Guid Id { get; set; }
        public Guid ReadMessageUserId { get; set; }
        public virtual User ReadMessageUser { get; set; }
        public Guid ReadMessageGroupMessageId { get; set; }
        public virtual GroupMessage ReadMessageGroupMessage { get; set; }
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
        //public virtual List<GroupMessage> Messages { get; set; } = new();

    }
}