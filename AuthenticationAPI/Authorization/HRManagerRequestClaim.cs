using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAPI.Authorization
{
    public class HRManagerRequestClaim :IAuthorizationRequirement
    {
        public HRManagerRequestClaim( int probationMonths)
        {
            ProbationMonths = probationMonths;
        }

        public int ProbationMonths { get; }

        public class  HRMangerProbationRequirementHandler : AuthorizationHandler<HRManagerRequestClaim>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerRequestClaim requirement)
            {
                // Check if the user has the required claim
                if (!context.User.HasClaim(c => c.Type == "EmploymentDate"))
                {
                    return Task.CompletedTask;
                }
             if(DateTime.TryParse(context.User.FindFirst(c => c.Type == "EmploymentDate")?.Value, out DateTime employmentDate))
                {
                    // Calculate the probation end date
                    var period = DateTime.Now - employmentDate;
                    // Check if the current date is past the probation end date
                    if (period.Days >= 30 * requirement.ProbationMonths)
                    {
                        context.Succeed(requirement);
                    }
                }
                return Task.CompletedTask;
            }
        }
    }
}
