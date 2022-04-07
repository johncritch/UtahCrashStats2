using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UtahCrashStats.Models;

namespace UtahCrashStats.Pages
{
    public class StoryHomeModel : PageModel
    {
        private readonly UtahCrashStats.Models.StoryDbContext _context;

        public StoryHomeModel(UtahCrashStats.Models.StoryDbContext context)
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
        }

        public Dictionary<string, int> filtersDict { get; set; }
        public Dictionary<string, string> filtersNamesDict { get; set; }

        public IList<Story> Story1 { get; set; }
        public IList<Story> Story2 { get; set; }
        public IList<Story> Story3 { get; set; }

        public int totalCrashes { get; set; }
        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public string searchTerm { get; set; }

        public async Task OnGetAsync(int p = 1, int s = 9)
        {
            totalCrashes = _context.Story.Count();
            pageSize = s;
            pageNum = p;

            Story1 = await _context.Story
                .Skip((p - 1) * 9)
                .Take(3)
                .ToListAsync();
            Story2 = await _context.Story
                .Skip(((p - 1) * 9) + 3)
                .Take(3)
                .ToListAsync();
            Story3 = await _context.Story
                .Skip(((p - 1) * 9) + 6)
                .Take(3)
                .ToListAsync();
        }
    }
}

