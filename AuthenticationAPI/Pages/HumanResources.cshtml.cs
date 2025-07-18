using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthenticationAPI.Pages
{
    [Authorize(Policy = "MustBelongToHR")]
    public class humanresourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
