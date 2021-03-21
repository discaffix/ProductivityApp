using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Model
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<SessionTag> Sessions { get; set; }
    }
}
