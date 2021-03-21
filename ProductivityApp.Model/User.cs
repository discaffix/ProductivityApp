using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductivityApp.Model
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(maximumLength:50, ErrorMessage = "Maximum 50 letters for FirstName Property.")]
        [ConcurrencyCheck]
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }
    
        /// <summary>
        /// A collection of the workspaces (one-to-many)
        /// </summary>
        public ICollection<Workspace> Workspaces { get; set; }


        /// <summary>
        /// A collection of the sessions (one-to-many)
        /// </summary>
        public ICollection<Session> Sessions { get; set; }

    }
}
