using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class GroupMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public Guid UserFromId { get; set; }
        public Guid GroupId { get; set; }
        public virtual User UserFrom { get; set; } = null!;
        public virtual Group Group { get; set; }
        public virtual List<ReadGroupMessage> ReadByUsers { get; set; } = new();

        //public virtual List<GroupMessage> Messages { get; set; } = new();

    }
}