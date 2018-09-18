using App.Areas.Auth.Models;
using App.Areas.Crm.Models;
using App.Areas.Documents.Models;
using App.Areas.Events.Models;
using App.Areas.Events.Repositories.Event;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Identification>()
                .HasOne(q => q.Contact)
                .WithMany(q => q.Identifications)
                .HasForeignKey(q => q.ContactId);

            builder.Entity<Phone>()
                .HasOne(q => q.Contact)
                .WithMany(q => q.Phones)
                .HasForeignKey(q => q.ContactId);

            builder.Entity<Email>()
                .HasOne(q => q.Contact)
                .WithMany(q => q.Emails)
                .HasForeignKey(q => q.ContactId);

            builder.Entity<Address>()
                .HasOne(q => q.Contact)
                .WithMany(q => q.Addresses)
                .HasForeignKey(q => q.ContactId);


            builder.Entity<ContactCTag>()
                .HasKey(t => new { t.ContactId, t.TagId });
            builder.Entity<ContactCTag>()
                .HasOne(pt => pt.Contact)
                .WithMany(p => p.ContactTags)
                .HasForeignKey(pt => pt.ContactId);
            builder.Entity<ContactCTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ContactTags)
                .HasForeignKey(pt => pt.TagId);

            builder.Entity<EventETag>()
                .HasKey(t => new { t.EventId, t.TagId });
            builder.Entity<EventETag>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.EventTags)
                .HasForeignKey(pt => pt.EventId);
            builder.Entity<EventETag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.EventTags)
                .HasForeignKey(pt => pt.TagId);
        }


        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CTag> CTags { get; set; }

        public DbSet<Identification> Identifications { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventItem> EventItems { get; set; }

        public DbSet<ETag> ETags { get; set; }

        public DbSet<Document> Documents { get; set; }
    }
}
