using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.Model;
using System;

namespace ProductivityApp.DataAccess
{
    public class ProductivityContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductivityContext"/> class.
        /// </summary>
        public ProductivityContext() { }

        /// <summary>
        /// Gets or sets the of users, clients, projectes, sessions, tags, and workspaces
        /// </summary>
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<SessionTag> SessionTags { get; set; }

        public ProductivityContext(DbContextOptions<ProductivityContext> options) : base (options) { }
        
        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "(localdb)\\MSSQLLocalDB",
                InitialCatalog = "ProductivityCatalog",
                IntegratedSecurity = true
            };

            optionsBuilder.UseSqlServer(builder.ConnectionString.ToString());
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionTag>()
                .HasKey(st => new { st.SessionId, st.TagId });
            modelBuilder.Entity<SessionTag>()
                .HasOne(st => st.Session)
                .WithMany(s => s.Tags)
                .HasForeignKey(st => st.SessionId);
            modelBuilder.Entity<SessionTag>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.Sessions)
                .HasForeignKey(st => st.TagId);
        }
    }
}
