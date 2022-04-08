using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtahCrashStats.Models;

namespace UtahCrashStats.Pages
{
    public class CreateModel : PageModel
    {
        private readonly UtahCrashStats.Models.CrashDbContext _context;

        public CreateModel(UtahCrashStats.Models.CrashDbContext context)
        {
            _context = context;

            filtersNamesDict = new Dictionary<string, string>();
            filtersNamesDict.Add("sev1", "No Injury");
            filtersNamesDict.Add("sev2", "Possible Injury");
            filtersNamesDict.Add("sev3", "Minor Injury");
            filtersNamesDict.Add("sev4", "Severe Injury");
            filtersNamesDict.Add("sev5", "Fatal Injury");
            filtersNamesDict.Add("work", "Work Zone");
            filtersNamesDict.Add("pede", "Pedestrian");
            filtersNamesDict.Add("bicy", "Bicyclist");
            filtersNamesDict.Add("moto", "Motorcycle");
            filtersNamesDict.Add("impr", "Improper Restraint");
            filtersNamesDict.Add("unre", "Unrestrained");
            filtersNamesDict.Add("duii", "DUI");
            filtersNamesDict.Add("inte", "Intersection");
            filtersNamesDict.Add("wild", "Wild Animal");
            filtersNamesDict.Add("dome", "Domestic Animal");
            filtersNamesDict.Add("roll", "Rollover");
            filtersNamesDict.Add("comm", "Commercial Vehicle");
            filtersNamesDict.Add("teen", "Teenage Driver");
            filtersNamesDict.Add("seni", "Senior Driver");
            filtersNamesDict.Add("nigh", "Nighttime");
            filtersNamesDict.Add("sing", "Single Vehicle");
            filtersNamesDict.Add("dist", "Distracted Driving");
            filtersNamesDict.Add("drow", "Drowsy Driving");
            filtersNamesDict.Add("road", "Roadway Departure");

            filtersDict = new Dictionary<string, int>();
            foreach (var filt in filtersNamesDict)
            {
                filtersDict.Add(filt.Key, 0);
            }
            detailsDict = filtersDict;
        }

        public Dictionary<string, int> filtersDict { get; set; }
        public Dictionary<string, string> filtersNamesDict { get; set; }
        public Dictionary<string, int> detailsDict { get; set; }

        public string detailsString { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Crash Crash { get; set; }
        //public String 

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string detailsInput = "")
        {
            detailsString = detailsInput;

            foreach (var detail in filtersNamesDict)
            {
                if (detailsInput != "" && detailsInput != null)
                {
                    if (detail.Key == detailsInput.Substring(0, 4))
                    {
                        detailsDict[detail.Key] = 1;
                        if (detailsInput.Length < 5)
                        {
                            detailsInput = detailsInput.Remove(0, 4);
                        }
                        else
                        {
                            detailsInput = detailsInput.Remove(0, 5);
                        }
                    }
                }
            }

            Crash.WORK_ZONE_RELATED = detailsDict["work"];
            Crash.PEDESTRIAN_INVOLVED = detailsDict["pede"];
            Crash.BICYCLIST_INVOLVED = detailsDict["bicy"];
            Crash.MOTORCYCLE_INVOLVED = detailsDict["moto"];
            Crash.IMPROPER_RESTRAINT = detailsDict["impr"];
            Crash.UNRESTRAINED = detailsDict["unre"];
            Crash.DUI = detailsDict["duii"];
            Crash.INTERSECTION_RELATED = detailsDict["inte"];
            Crash.WILD_ANIMAL_RELATED = detailsDict["wild"];
            Crash.DOMESTIC_ANIMAL_RELATED = detailsDict["dome"];
            Crash.OVERTURN_ROLLOVER = detailsDict["roll"];
            Crash.COMMERCIAL_MOTOR_VEH_INVOLVED = detailsDict["comm"];
            Crash.TEENAGE_DRIVER_INVOLVED = detailsDict["teen"];
            Crash.OLDER_DRIVER_INVOLVED = detailsDict["seni"];
            Crash.NIGHT_DARK_CONDITION = detailsDict["nigh"];
            Crash.SINGLE_VEHICLE = detailsDict["sing"];
            Crash.DISTRACTED_DRIVING = detailsDict["dist"];
            Crash.DROWSY_DRIVING = detailsDict["drow"];
            Crash.ROADWAY_DEPARTURE = detailsDict["road"];

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Crash.Add(Crash);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Crashes");
        }
    }
}
