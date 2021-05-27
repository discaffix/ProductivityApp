using System.Collections.Generic;

namespace ProductivityApp.Model
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public ICollection<SessionTag> Sessions { get; set; }
    }
}
