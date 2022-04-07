using System;
using System.ComponentModel.DataAnnotations;

namespace UtahCrashStats.Models
{
    public class Crash
    {
        [Key]
        [Required]
        public int CRASH_ID { get; set; }
        [Required]
        public DateTime CRASH_DATETIME { get; set; }
        public int ROUTE { get; set; }
        public double MILEPOINT { get; set; }
        public double LAT_UTM_Y { get; set; }
        public double LONG_UTM_X { get; set; }
        [Required]
        public string MAIN_ROAD_NAME { get; set; }
        [Required]
        public string CITY { get; set; }
        [Required]
        public string COUNTY_NAME { get; set; }
        [Required]
        public int CRASH_SEVERITY_ID { get; set; }
        public int WORK_ZONE_RELATED { get; set; }
        public int PEDESTRIAN_INVOLVED { get; set; }
        public int BICYCLIST_INVOLVED { get; set; }
        public int MOTORCYCLE_INVOLVED { get; set; }
        public int IMPROPER_RESTRAINT { get; set; }
        public int UNRESTRAINED { get; set; }
        public int DUI { get; set; }
        public int INTERSECTION_RELATED { get; set; }
        public int WILD_ANIMAL_RELATED { get; set; }
        public int DOMESTIC_ANIMAL_RELATED { get; set; }
        public int OVERTURN_ROLLOVER { get; set; }
        public int COMMERCIAL_MOTOR_VEH_INVOLVED { get; set; }
        public int TEENAGE_DRIVER_INVOLVED { get; set; }
        public int OLDER_DRIVER_INVOLVED { get; set; }
        public int NIGHT_DARK_CONDITION { get; set; }
        public int SINGLE_VEHICLE { get; set; }
        public int DISTRACTED_DRIVING { get; set; }
        public int DROWSY_DRIVING { get; set; }
        public int ROADWAY_DEPARTURE { get; set; }
    }
}
