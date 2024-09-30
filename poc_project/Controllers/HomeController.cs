using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using poc_project.Data;
using poc_project.Models.Entity;
using poc_project.Models.ViewModels;

namespace poc_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult Index()
        {
            HttpContext.Session.Clear();

            var viewModel = new QuestionnaireViewModel
            {
                MaterialIssues = dbContext.MaterialIssues.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Questionnaire()
        {

            var organization = HttpContext.Session.GetString("Organization");

            if (organization == null)
            {

               return RedirectToAction("Index");
            }

            var stakeholder = dbContext.Stakeholders.FirstOrDefault(s => s.Organization == organization);

            if (stakeholder == null)
            {
                // If stakeholder does not exist, create a new one
                stakeholder = new Stakeholder
                {
                    Organization = organization
                };
            }


            // Fetch all material issues
            var allIssues = dbContext.MaterialIssues.ToList();

  
            // Create a new view model instance
            var viewModel = new QuestionnaireViewModel
            {
                Stakeholder = stakeholder ?? new Stakeholder(), 
                MaterialIssues = allIssues,       
                Responses = new List<Draft>() 
            };

            var drafts = dbContext.Drafts.Where(d => d.StakeholderId == stakeholder.StakeholderId).ToList();


            // Initialize Responses with empty ResponseRelevance objects for each issue
            foreach (var issue in allIssues)
            {
                var draft = drafts.FirstOrDefault(d => d.MaterialIssueId == issue.IssueId);

                viewModel.Responses.Add(new Draft
                {
                    MaterialIssueId = issue.IssueId ,
                    RelevanceScore = draft?.RelevanceScore ?? 0,
                    Comments = draft?.Comments ?? string.Empty
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> QuestionnaireSubmit(QuestionnaireViewModel model, string action)
        {
            if (action == "submit")
            {
                // Check if the stakeholder exists in the database
                var existingStakeholder = await dbContext.Stakeholders
                    .FirstOrDefaultAsync(s => s.Organization == model.Stakeholder.Organization);

                if (existingStakeholder == null)
                {
                    // If stakeholder doesn't exist, create a new one
                    var newStakeholder = new Stakeholder
                    {
                        Name = model.Stakeholder.Name,
                        Organization = model.Stakeholder.Organization,
                        Role = model.Stakeholder.Role,
                        Category = model.Stakeholder.Category
                    };

                    dbContext.Stakeholders.Add(newStakeholder);
                    await dbContext.SaveChangesAsync(); 

                    // Ensure StakeholderId is set
                    model.Stakeholder.StakeholderId = newStakeholder.StakeholderId;
                }
                else
                {
                    // Use the existing stakeholder
                    model.Stakeholder.StakeholderId = existingStakeholder.StakeholderId;
                }

                foreach (var response in model.Responses)
                {
                    if (response.RelevanceScore > 0)
                    {
                        var responseRelevance = new ResponseRelevance
                        {
                            StakeholderId = model.Stakeholder.StakeholderId,
                            IssueId = response.MaterialIssueId,
                            RelevanceScore = response.RelevanceScore ?? 0,
                            Comments = response.Comments ?? string.Empty
                        };

                        dbContext.ResponseRelevances.Add(responseRelevance);
                    }
                }

                var drafts = dbContext.Drafts
                .Where(d => d.StakeholderId == model.Stakeholder.StakeholderId)
                .ToList();

                if (drafts.Any())
                {
                    dbContext.Drafts.RemoveRange(drafts);
                }


                await dbContext.SaveChangesAsync(); 
                return RedirectToAction("Response");
            }

            if (action == "save")
            {
                var stakeholder = await dbContext.Stakeholders
                    .FirstOrDefaultAsync(s => s.Organization == model.Stakeholder.Organization);

                if (stakeholder == null)
                {
                    var newStakeholder = new Stakeholder
                    {
                        Name = model.Stakeholder.Name,
                        Organization = model.Stakeholder.Organization,
                        Role = model.Stakeholder.Role,
                        Category = model.Stakeholder.Category
                    };

                    // Save the new stakeholder
                    dbContext.Stakeholders.Add(newStakeholder);
                    await dbContext.SaveChangesAsync(); // Ensure this is saved first

                    // Set the StakeholderId for the model
                    model.Stakeholder.StakeholderId = newStakeholder.StakeholderId;
                }
                else
                {
                    model.Stakeholder.StakeholderId = stakeholder.StakeholderId;
                }

                // Prepare to save drafts
                foreach (var response in model.Responses)
                {
                    if (response.RelevanceScore > 0)
                    {
                        var newDraft = new Draft
                        {
                            StakeholderId = model.Stakeholder.StakeholderId,
                            MaterialIssueId = response.MaterialIssueId,
                            RelevanceScore = response.RelevanceScore ?? 0,
                            Comments = response.Comments ?? string.Empty
                        };
                        dbContext.Drafts.Add(newDraft);
                    }
                }


                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(model);
        }



        [HttpPost]
        public IActionResult CheckOrganization(string organization)
        {

            HttpContext.Session.Clear();

            HttpContext.Session.SetString("Organization", organization);

            var stakeholderOrganization = dbContext.Stakeholders.FirstOrDefault(s => s.Organization == organization);

            if (stakeholderOrganization == null)
            {
                return RedirectToAction("Questionnaire");
            }

            var hasDrafts = dbContext.Drafts.Any(d => d.StakeholderId == stakeholderOrganization.StakeholderId);

            if (hasDrafts)
            {
                return RedirectToAction("Questionnaire");
            }
          
            return RedirectToAction("Response");

        }


        public IActionResult Response()
        {
            var organization = HttpContext.Session.GetString("Organization");

            var responses = dbContext.ResponseRelevances
            .Join(dbContext.Stakeholders,
                  rr => rr.StakeholderId,
                  s => s.StakeholderId,
                  (rr, s) => new { rr, s })
            .Join(dbContext.MaterialIssues,
                  combined => combined.rr.IssueId,
                  m => m.IssueId,
                  (combined, m) => new ResponseViewModel
                  {
                      Name = combined.s.Name,
                      Organization = combined.s.Organization,
                      Role = combined.s.Role,
                      Category = combined.s.Category,
                      RelevanceScore = combined.rr.RelevanceScore,
                      Comments = combined.rr.Comments,
                      IssueName = m.IssueName,
                      IssueCategory = m.IssueCategory
                  })
             .Where(response => response.Organization == organization) // Filter by organization
            .GroupBy(response => new { response.IssueName, response.IssueCategory }) // Group by issue name and category
            .Select(g => new ResponseViewModel
            {
                Name = g.FirstOrDefault().Name,
                Organization = g.FirstOrDefault().Organization,
                Role = g.FirstOrDefault().Role,
                Category = g.FirstOrDefault().Category,
                RelevanceScore = (int)g.Average(x => x.RelevanceScore), // Calculate the average relevance score
                Comments = string.Join("<br/>", g
                .Where(x => !string.IsNullOrEmpty(x.Comments))
                .Select(x => $"{x.Name} ({x.Role}): {x.Comments}")),
                IssueName = g.Key.IssueName,
                IssueCategory = g.Key.IssueCategory
            })
        .OrderByDescending(response => response.RelevanceScore) // Sort by average RelevanceScore descending
        .ToList();


            return View(responses);
        }

    }
}
