using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Model
{
    public class Session
    {
        public int SessionId { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public User User { get; set; }

        public Project Project { get; set; }

        public ICollection<SessionTag> Tags { get; set; }
    }
}
