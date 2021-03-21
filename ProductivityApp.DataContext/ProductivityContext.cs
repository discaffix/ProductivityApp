using Microsoft.EntityFrameworkCore;
using System;

namespace ProductivityApp.DataAccess
{
    public class ProductivityContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductivityContext"/> class.
        /// </summary>
        public ProductivityContext() { }
    }
}
