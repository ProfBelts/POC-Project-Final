namespace poc_project.Models.Entity
{
    public class Draft
    {
        public int DraftId { get; set; }
        public int? RelevanceScore { get; set; }
        public string Comments { get; set; }

        // Foreign keys
        public int StakeholderId { get; set; }
        public Stakeholder Stakeholder { get; set; }

        public int MaterialIssueId { get; set; }
        public MaterialIssue MaterialIssue { get; set; }

    }
}
