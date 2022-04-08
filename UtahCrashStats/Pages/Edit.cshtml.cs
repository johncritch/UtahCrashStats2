using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UtahCrashStats.Models;

namespace UtahCrashStats.Pages
{
    public class EditModel : PageModel
    {
        private readonly UtahCrashStats.Models.CrashDbContext _context;

        public EditModel(UtahCrashStats.Models.CrashDbContext context)
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
            detailsDict = new Dictionary<string, int>();
        }

        public Dictionary<string, int> filtersDict { get; set; }
        public Dictionary<string, string> filtersNamesDict { get; set; }
        public Dictionary<string, int> detailsDict { get; set; }

        public string detailsString { get; set; }

        [BindProperty]
        public Crash Crash { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Crash = await _context.Crash.FirstOrDefaultAsync(m => m.CRASH_ID == id);

            detailsDict.Add("sev1", 0);
            detailsDict.Add("sev2", 0);
            detailsDict.Add("sev3", 0);
            detailsDict.Add("sev4", 0);
            detailsDict.Add("sev5", 0);
            detailsDict.Add("work", Crash.WORK_ZONE_RELATED);
            detailsDict.Add("pede", Crash.PEDESTRIAN_INVOLVED);
            detailsDict.Add("bicy", Crash.BICYCLIST_INVOLVED);
            detailsDict.Add("moto", Crash.MOTORCYCLE_INVOLVED);
            detailsDict.Add("impr", Crash.IMPROPER_RESTRAINT);
            detailsDict.Add("unre", Crash.UNRESTRAINED);
            detailsDict.Add("duii", Crash.DUI);
            detailsDict.Add("inte", Crash.INTERSECTION_RELATED);
            detailsDict.Add("wild", Crash.WILD_ANIMAL_RELATED);
            detailsDict.Add("dome", Crash.DOMESTIC_ANIMAL_RELATED);
            detailsDict.Add("roll", Crash.OVERTURN_ROLLOVER);
            detailsDict.Add("comm", Crash.COMMERCIAL_MOTOR_VEH_INVOLVED);
            detailsDict.Add("teen", Crash.TEENAGE_DRIVER_INVOLVED);
            detailsDict.Add("seni", Crash.OLDER_DRIVER_INVOLVED);
            detailsDict.Add("nigh", Crash.NIGHT_DARK_CONDITION);
            detailsDict.Add("sing", Crash.SINGLE_VEHICLE);
            detailsDict.Add("dist", Crash.DISTRACTED_DRIVING);
            detailsDict.Add("drow", Crash.DROWSY_DRIVING);
            detailsDict.Add("road", Crash.ROADWAY_DEPARTURE);

            if (Crash == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id, string detailsInput = "")
        {
            Crash.CRASH_ID = id;
            detailsString = detailsInput;

            foreach (var filt in filtersNamesDict)
            {
                detailsDict.Add(filt.Key, 0);
            }

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

            _context.Attach(Crash).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrashExists(Crash.CRASH_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Crashes");
        }

        private bool CrashExists(int id)
        {
            return _context.Crash.Any(e => e.CRASH_ID == id);
        }
    }
}

