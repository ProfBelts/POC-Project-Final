namespace poc_project.Models.Entity
{
    public class MaterialIssue
    {
        public int IssueId { get; set; }
        public string IssueName { get; set; }
        public string IssueCategory { get; set; }

        // Navigation properties
        public ICollection<ResponseRelevance> ResponseRelevances { get; set; }
        public ICollection<Draft> Drafts { get; set; }
    }
}
