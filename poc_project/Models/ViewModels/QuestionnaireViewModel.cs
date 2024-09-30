using poc_project.Models.Entity;

namespace poc_project.Models.ViewModels
{
    public class QuestionnaireViewModel
    {
        public Stakeholder Stakeholder { get; set; } = new Stakeholder();
        public List<Draft> Responses { get; set; } = new List<Draft>();
        public List<MaterialIssue> MaterialIssues { get; set; }
    }
}
