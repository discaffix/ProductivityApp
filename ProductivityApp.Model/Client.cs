using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Model
{
    public class Client
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }
        
        public string Name { get; set; }
        
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// The property that defines the one-to-many relationship between project and client.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
    }
}   
