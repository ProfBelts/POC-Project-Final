namespace poc_project.Models.Entity
{
    public class Stakeholder
    {
        public int StakeholderId { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public string Role { get; set; }
        public string Category { get; set; }

        // Navigation properties
        public ICollection<ResponseRelevance> ResponseRelevances { get; set; }
        public ICollection<Draft> Drafts { get; set; }
    }
}
