using System;
using System.ComponentModel.DataAnnotations;

namespace InfomationManager.Models
{
    public class SystemUser
    {
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
