using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductivityApp.Model
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public ICollection<SessionTag> Tags { get; set; } = new List<SessionTag>();
    }
}
