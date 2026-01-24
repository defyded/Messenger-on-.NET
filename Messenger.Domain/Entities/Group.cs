using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserOwnerId { get; set; }
        [NotMapped]
        public virtual User UserOwner { get; set; } = null!;
        public virtual List<User> Users { get; set; } = new();
        public virtual List<GroupMessage> Messages { get; set; } = new();

        //public virtual List<GroupMessage> Messages { get; set; } = new();

    }
}