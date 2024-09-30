using Microsoft.EntityFrameworkCore;
using poc_project.Models.Entity;

namespace poc_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Stakeholder> Stakeholders { get; set; }
        public DbSet<MaterialIssue> MaterialIssues { get; set; }
        public DbSet<ResponseRelevance> ResponseRelevances { get; set; }
        public DbSet<Draft> Drafts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Stakeholder
            modelBuilder.Entity<Stakeholder>()
                .HasKey(s => s.StakeholderId);

            // Material Issue
            modelBuilder.Entity<MaterialIssue>()
                .HasKey(m => m.IssueId);

            // ResponseRelevance
            modelBuilder.Entity<ResponseRelevance>()
                .HasKey(rr => rr.ResponseId);

            modelBuilder.Entity<ResponseRelevance>()
                .HasOne(rr => rr.Stakeholder)
                .WithMany(s => s.ResponseRelevances)
                .HasForeignKey(rr => rr.StakeholderId);

            modelBuilder.Entity<ResponseRelevance>()
                .HasOne(rr => rr.Issue)
                .WithMany(mi => mi.ResponseRelevances)
                .HasForeignKey(rr => rr.IssueId);

            // Draft
            modelBuilder.Entity<Draft>()
                .HasKey(d => d.DraftId);

            modelBuilder.Entity<Draft>()
                .HasOne(d => d.Stakeholder)
                .WithMany(s => s.Drafts)
                .HasForeignKey(d => d.StakeholderId);

            modelBuilder.Entity<Draft>()
                .HasOne(d => d.MaterialIssue)
                .WithMany(mi => mi.Drafts)
                .HasForeignKey(d => d.MaterialIssueId);
        }

    }
}
