using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using UtahCrashStats.Models;

namespace UtahCrashStats.Pages
{
    public class CrashesModel : PageModel
    {
        private readonly UtahCrashStats.Models.CrashDbContext context;
        private InferenceSession session;

        public CrashesModel(UtahCrashStats.Models.CrashDbContext _context, InferenceSession _session)
        {
            context = _context;
            session = _session;

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

        public List<Crash> crashes { get; set; }

        public int totalCrashes { get; set; }

        public int pageNum { get; set; }

        public int pageSize { get; set; }

        public string searchTerm { get; set; }

        public string filterString { get; set; }

        public int predictedSeverity { get; set; }

        public Dictionary<string, int> filtersDict { get; set; }
        public Dictionary<string, string> filtersNamesDict { get; set; }

        public PredictionInput data { get; set; }

        public bool includesSeverity { get; set; }

        public void OnGet(string filterInput = "", int p = 1, int s = 10, string search = "")
        {
            if (search == null)
            {
                search = "";
            }
            searchTerm = search;
            filterString = filterInput;

            foreach (var filt in filtersNamesDict)
            {
                if (filterInput != "" && filterInput != null)
                {
                    if (filt.Key == filterInput.Substring(0, 4))
                    {
                        filtersDict[filt.Key] = 1;
                        if (filterInput.Length < 5)
                        {
                            filterInput = filterInput.Remove(0, 4);
                        }
                        else
                        {
                            filterInput = filterInput.Remove(0, 5);
                        }
                    }
                }
            }

            data = new PredictionInput();
            
            data.Bicyclist = (float)0;
            
            if (searchTerm.Contains("salt lake city"))
            {
                data.CitySaltLake = 1;
                data.CountySaltLake = 1;
            }
            else
            {
                data.CitySaltLake = 0;
                data.CountySaltLake = 0;
            }
            if (searchTerm.Contains("west valley"))
            {
                data.CityWestValley = 1;
                data.CountySaltLake = 1;
            }
            else
            {
                data.CityWestValley = 1;
                data.CountySaltLake = 1;
            }
            if (data.CitySaltLake == 1 || data.CityWestValley == 1)
            {
                data.CityOther = 0;
            }
            else
            {
                data.CityOther = 1;
            }
            if (searchTerm.Contains("Weber"))
            {
                data.CountyWeber = 1;
            }
            else
            {
                data.CountyWeber = 0;
            }
            data.CountyUtah = 0;
            if (data.CountyWeber == 1 || data.CountySaltLake == 1)
            {
                data.CountyOther = 0;
            }
            else
            {
                data.CountyOther = 1;
            }
          
            data.ComercialVehicle = filtersDict["comm"];
            data.DistractedDriving = filtersDict["dist"];
            data.DomesticAnimal = filtersDict["dome"];
            data.DrowsyDriving = filtersDict["drow"];
            data.DUI = filtersDict["duii"];
            data.ImproperRestraint = filtersDict["impr"];
            data.Intersection = filtersDict["inte"];
            data.Milepoint = 0;
            data.Motorcycle = filtersDict["moto"];
            data.NightTime = filtersDict["nigh"];
            data.OlderDriver = filtersDict["seni"];
            data.Pedestrian = filtersDict["pede"];
            data.RoadwayDeparture = filtersDict["road"];
            data.Rollover = filtersDict["roll"];
            if (searchTerm.Contains("89"))
            {
                data.Route89 = 1;
            }
            else
            {
                data.Route89 = 0;
            }
            data.SingleVehicle = filtersDict["sing"];
            data.TeanageDriver = filtersDict["teen"];
            data.Unrestrained = filtersDict["unre"];
            data.WildAnimal = filtersDict["wild"];
            data.WorkZone = filtersDict["work"];

            int severity = 0;
            if (filtersDict["sev1"] == 1)
            {
                severity = 1;
            }
            else if (filtersDict["sev2"] == 1)
            {
                severity = 2;
            }
            else if (filtersDict["sev3"] == 1)
            {
                severity = 3;
            }
            else if (filtersDict["sev4"] == 1)
            {
                severity = 4;
            }
            else if (filtersDict["sev5"] == 1)
            {
                severity = 5;
            }

            if (severity == 0 && (searchTerm != "" || filterString != ""))
            {
                includesSeverity = false;
                crashes = context.Crash
                    .Where(
                    x => (x.CITY.Contains(searchTerm) ||
                    x.COUNTY_NAME.Contains(searchTerm) ||
                    x.MAIN_ROAD_NAME.Contains(searchTerm)) &&
                    (x.WORK_ZONE_RELATED.Equals(filtersDict["work"]) || x.WORK_ZONE_RELATED.Equals(1)) &&
                    (x.PEDESTRIAN_INVOLVED.Equals(filtersDict["pede"]) || x.PEDESTRIAN_INVOLVED.Equals(1)) &&
                    (x.BICYCLIST_INVOLVED.Equals(filtersDict["bicy"]) || x.BICYCLIST_INVOLVED.Equals(1)) &&
                    (x.MOTORCYCLE_INVOLVED.Equals(filtersDict["moto"]) || x.MOTORCYCLE_INVOLVED.Equals(1)) &&
                    (x.IMPROPER_RESTRAINT.Equals(filtersDict["impr"]) || x.IMPROPER_RESTRAINT.Equals(1)) &&
                    (x.UNRESTRAINED.Equals(filtersDict["unre"]) || x.UNRESTRAINED.Equals(1)) &&
                    (x.DUI.Equals(filtersDict["duii"]) || x.DUI.Equals(1)) &&
                    (x.INTERSECTION_RELATED.Equals(filtersDict["inte"]) || x.INTERSECTION_RELATED.Equals(1)) &&
                    (x.WILD_ANIMAL_RELATED.Equals(filtersDict["wild"]) || x.WILD_ANIMAL_RELATED.Equals(1)) &&
                    (x.DOMESTIC_ANIMAL_RELATED.Equals(filtersDict["dome"]) || x.DOMESTIC_ANIMAL_RELATED.Equals(1)) &&
                    (x.OVERTURN_ROLLOVER.Equals(filtersDict["roll"]) || x.OVERTURN_ROLLOVER.Equals(1)) &&
                    (x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(filtersDict["comm"]) || x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(1)) &&
                    (x.TEENAGE_DRIVER_INVOLVED.Equals(filtersDict["teen"]) || x.TEENAGE_DRIVER_INVOLVED.Equals(1)) &&
                    (x.OLDER_DRIVER_INVOLVED.Equals(filtersDict["seni"]) || x.OLDER_DRIVER_INVOLVED.Equals(1)) &&
                    (x.NIGHT_DARK_CONDITION.Equals(filtersDict["nigh"]) || x.NIGHT_DARK_CONDITION.Equals(1)) &&
                    (x.SINGLE_VEHICLE.Equals(filtersDict["sing"]) || x.SINGLE_VEHICLE.Equals(1)) &&
                    (x.DISTRACTED_DRIVING.Equals(filtersDict["dist"]) || x.DISTRACTED_DRIVING.Equals(1)) &&
                    (x.DROWSY_DRIVING.Equals(filtersDict["drow"]) || x.DROWSY_DRIVING.Equals(1)) &&
                    (x.ROADWAY_DEPARTURE.Equals(filtersDict["road"]) || x.ROADWAY_DEPARTURE.Equals(1))
                    )
                    .OrderByDescending(x => x.CRASH_DATETIME)
                    .Skip((p - 1) * s)
                    .Take(s)
                    .ToList();
                totalCrashes = context.Crash
                    .Where(
                    x => (x.CITY.Contains(searchTerm) ||
                    x.COUNTY_NAME.Contains(searchTerm) ||
                    x.MAIN_ROAD_NAME.Contains(searchTerm)) &&
                    (x.WORK_ZONE_RELATED.Equals(filtersDict["work"]) || x.WORK_ZONE_RELATED.Equals(1)) &&
                    (x.PEDESTRIAN_INVOLVED.Equals(filtersDict["pede"]) || x.PEDESTRIAN_INVOLVED.Equals(1)) &&
                    (x.BICYCLIST_INVOLVED.Equals(filtersDict["bicy"]) || x.BICYCLIST_INVOLVED.Equals(1)) &&
                    (x.MOTORCYCLE_INVOLVED.Equals(filtersDict["moto"]) || x.MOTORCYCLE_INVOLVED.Equals(1)) &&
                    (x.IMPROPER_RESTRAINT.Equals(filtersDict["impr"]) || x.IMPROPER_RESTRAINT.Equals(1)) &&
                    (x.UNRESTRAINED.Equals(filtersDict["unre"]) || x.UNRESTRAINED.Equals(1)) &&
                    (x.DUI.Equals(filtersDict["duii"]) || x.DUI.Equals(1)) &&
                    (x.INTERSECTION_RELATED.Equals(filtersDict["inte"]) || x.INTERSECTION_RELATED.Equals(1)) &&
                    (x.WILD_ANIMAL_RELATED.Equals(filtersDict["wild"]) || x.WILD_ANIMAL_RELATED.Equals(1)) &&
                    (x.DOMESTIC_ANIMAL_RELATED.Equals(filtersDict["dome"]) || x.DOMESTIC_ANIMAL_RELATED.Equals(1)) &&
                    (x.OVERTURN_ROLLOVER.Equals(filtersDict["roll"]) || x.OVERTURN_ROLLOVER.Equals(1)) &&
                    (x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(filtersDict["comm"]) || x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(1)) &&
                    (x.TEENAGE_DRIVER_INVOLVED.Equals(filtersDict["teen"]) || x.TEENAGE_DRIVER_INVOLVED.Equals(1)) &&
                    (x.OLDER_DRIVER_INVOLVED.Equals(filtersDict["seni"]) || x.OLDER_DRIVER_INVOLVED.Equals(1)) &&
                    (x.NIGHT_DARK_CONDITION.Equals(filtersDict["nigh"]) || x.NIGHT_DARK_CONDITION.Equals(1)) &&
                    (x.SINGLE_VEHICLE.Equals(filtersDict["sing"]) || x.SINGLE_VEHICLE.Equals(1)) &&
                    (x.DISTRACTED_DRIVING.Equals(filtersDict["dist"]) || x.DISTRACTED_DRIVING.Equals(1)) &&
                    (x.DROWSY_DRIVING.Equals(filtersDict["drow"]) || x.DROWSY_DRIVING.Equals(1)) &&
                    (x.ROADWAY_DEPARTURE.Equals(filtersDict["road"]) || x.ROADWAY_DEPARTURE.Equals(1))
                    )
                    .Count();
            }
            else if (severity != 0 && (searchTerm != "" || filterString != ""))
            {
                includesSeverity = true;
                crashes = context.Crash
                    .Where(
                    x => (x.CITY.Contains(searchTerm) ||
                    x.COUNTY_NAME.Contains(searchTerm) ||
                    x.MAIN_ROAD_NAME.Contains(searchTerm)) &&
                    x.CRASH_SEVERITY_ID.Equals(severity) &&
                    (x.WORK_ZONE_RELATED.Equals(filtersDict["work"]) || x.WORK_ZONE_RELATED.Equals(1)) &&
                    (x.PEDESTRIAN_INVOLVED.Equals(filtersDict["pede"]) || x.PEDESTRIAN_INVOLVED.Equals(1)) &&
                    (x.BICYCLIST_INVOLVED.Equals(filtersDict["bicy"]) || x.BICYCLIST_INVOLVED.Equals(1)) &&
                    (x.MOTORCYCLE_INVOLVED.Equals(filtersDict["moto"]) || x.MOTORCYCLE_INVOLVED.Equals(1)) &&
                    (x.IMPROPER_RESTRAINT.Equals(filtersDict["impr"]) || x.IMPROPER_RESTRAINT.Equals(1)) &&
                    (x.UNRESTRAINED.Equals(filtersDict["unre"]) || x.UNRESTRAINED.Equals(1)) &&
                    (x.DUI.Equals(filtersDict["duii"]) || x.DUI.Equals(1)) &&
                    (x.INTERSECTION_RELATED.Equals(filtersDict["inte"]) || x.INTERSECTION_RELATED.Equals(1)) &&
                    (x.WILD_ANIMAL_RELATED.Equals(filtersDict["wild"]) || x.WILD_ANIMAL_RELATED.Equals(1)) &&
                    (x.DOMESTIC_ANIMAL_RELATED.Equals(filtersDict["dome"]) || x.DOMESTIC_ANIMAL_RELATED.Equals(1)) &&
                    (x.OVERTURN_ROLLOVER.Equals(filtersDict["roll"]) || x.OVERTURN_ROLLOVER.Equals(1)) &&
                    (x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(filtersDict["comm"]) || x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(1)) &&
                    (x.TEENAGE_DRIVER_INVOLVED.Equals(filtersDict["teen"]) || x.TEENAGE_DRIVER_INVOLVED.Equals(1)) &&
                    (x.OLDER_DRIVER_INVOLVED.Equals(filtersDict["seni"]) || x.OLDER_DRIVER_INVOLVED.Equals(1)) &&
                    (x.NIGHT_DARK_CONDITION.Equals(filtersDict["nigh"]) || x.NIGHT_DARK_CONDITION.Equals(1)) &&
                    (x.SINGLE_VEHICLE.Equals(filtersDict["sing"]) || x.SINGLE_VEHICLE.Equals(1)) &&
                    (x.DISTRACTED_DRIVING.Equals(filtersDict["dist"]) || x.DISTRACTED_DRIVING.Equals(1)) &&
                    (x.DROWSY_DRIVING.Equals(filtersDict["drow"]) || x.DROWSY_DRIVING.Equals(1)) &&
                    (x.ROADWAY_DEPARTURE.Equals(filtersDict["road"]) || x.ROADWAY_DEPARTURE.Equals(1))
                    )
                    .OrderByDescending(x => x.CRASH_DATETIME)
                    .Skip((p - 1) * s)
                    .Take(s)
                    .ToList();
                totalCrashes = context.Crash
                    .Where(
                    x => (x.CITY.Contains(searchTerm) ||
                    x.COUNTY_NAME.Contains(searchTerm) ||
                    x.MAIN_ROAD_NAME.Contains(searchTerm)) &&
                    x.CRASH_SEVERITY_ID.Equals(severity) &&
                    (x.WORK_ZONE_RELATED.Equals(filtersDict["work"]) || x.WORK_ZONE_RELATED.Equals(1)) &&
                    (x.PEDESTRIAN_INVOLVED.Equals(filtersDict["pede"]) || x.PEDESTRIAN_INVOLVED.Equals(1)) &&
                    (x.BICYCLIST_INVOLVED.Equals(filtersDict["bicy"]) || x.BICYCLIST_INVOLVED.Equals(1)) &&
                    (x.MOTORCYCLE_INVOLVED.Equals(filtersDict["moto"]) || x.MOTORCYCLE_INVOLVED.Equals(1)) &&
                    (x.IMPROPER_RESTRAINT.Equals(filtersDict["impr"]) || x.IMPROPER_RESTRAINT.Equals(1)) &&
                    (x.UNRESTRAINED.Equals(filtersDict["unre"]) || x.UNRESTRAINED.Equals(1)) &&
                    (x.DUI.Equals(filtersDict["duii"]) || x.DUI.Equals(1)) &&
                    (x.INTERSECTION_RELATED.Equals(filtersDict["inte"]) || x.INTERSECTION_RELATED.Equals(1)) &&
                    (x.WILD_ANIMAL_RELATED.Equals(filtersDict["wild"]) || x.WILD_ANIMAL_RELATED.Equals(1)) &&
                    (x.DOMESTIC_ANIMAL_RELATED.Equals(filtersDict["dome"]) || x.DOMESTIC_ANIMAL_RELATED.Equals(1)) &&
                    (x.OVERTURN_ROLLOVER.Equals(filtersDict["roll"]) || x.OVERTURN_ROLLOVER.Equals(1)) &&
                    (x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(filtersDict["comm"]) || x.COMMERCIAL_MOTOR_VEH_INVOLVED.Equals(1)) &&
                    (x.TEENAGE_DRIVER_INVOLVED.Equals(filtersDict["teen"]) || x.TEENAGE_DRIVER_INVOLVED.Equals(1)) &&
                    (x.OLDER_DRIVER_INVOLVED.Equals(filtersDict["seni"]) || x.OLDER_DRIVER_INVOLVED.Equals(1)) &&
                    (x.NIGHT_DARK_CONDITION.Equals(filtersDict["nigh"]) || x.NIGHT_DARK_CONDITION.Equals(1)) &&
                    (x.SINGLE_VEHICLE.Equals(filtersDict["sing"]) || x.SINGLE_VEHICLE.Equals(1)) &&
                    (x.DISTRACTED_DRIVING.Equals(filtersDict["dist"]) || x.DISTRACTED_DRIVING.Equals(1)) &&
                    (x.DROWSY_DRIVING.Equals(filtersDict["drow"]) || x.DROWSY_DRIVING.Equals(1)) &&
                    (x.ROADWAY_DEPARTURE.Equals(filtersDict["road"]) || x.ROADWAY_DEPARTURE.Equals(1))
                    )
                    .Count();
            } else
            {
                crashes = context.Crash
                    .OrderByDescending(x => x.CRASH_DATETIME)
                    .Skip((p - 1) * s)
                    .Take(s)
                    .ToList();
                totalCrashes = context.Crash
                    .Count();
            }
<<<<<<< HEAD

            var result = session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor()) 
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new PredictionOutput { PredictedSeverity = score.First() };
            result.Dispose();

            predictedSeverity = (int)Math.Round(prediction.PredictedSeverity);
=======
            pageSize = s;
            pageNum = p;
>>>>>>> origin
        }
    }
}

