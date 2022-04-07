using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UtahCrashStats.Pages
{
    public class AboutMeModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        //public Account(UserManager<IdentityUser> um, SignInManager<IdentityUser> sim)
        //{
        //    userManager = um;
        //    signInManager = sim;
        //}


        public void OnGet()
        {
        }
    }
}
