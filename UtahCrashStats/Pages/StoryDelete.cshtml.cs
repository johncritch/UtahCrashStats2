﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UtahCrashStats.Models;

namespace UtahCrashStats.Pages
{
    public class StoryDeleteModel : PageModel
    {
        private readonly UtahCrashStats.Models.StoryDbContext _context;

        public StoryDeleteModel(UtahCrashStats.Models.StoryDbContext context)
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

        [BindProperty]
        public Story Story { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Story = await _context.Story.FirstOrDefaultAsync(m => m.STORY_ID == id);

            if (Story == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Story = await _context.Story.FindAsync(id);

            if (Story != null)
            {
                _context.Story.Remove(Story);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./StoryHome");
        }
    }
}

