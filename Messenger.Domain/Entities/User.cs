using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        // public string phoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Descrption { get; set; }
        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeenAt { get; set; }
        public List<UserDevice> UserDevices { get; set; } = new();
        //public List<Chat> Chats { get; set; } = new();
        public List<Group> Groups { get; set; } = new(); 
    }
}