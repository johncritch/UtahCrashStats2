using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCrashStats.Models
{
    public class PredictionInput
    {
        public float Milepoint { get; set; }
        public float WorkZone { get; set; }
        public float Pedestrian { get; set; }
        public float Bicyclist { get; set; }
        public float Motorcycle { get; set; }
        public float ImproperRestraint { get; set; }
        public float Unrestrained { get; set; }
        public float DUI { get; set; }
        public float Intersection { get; set; }
        public float WildAnimal { get; set; }
        public float DomesticAnimal { get; set; }
        public float Rollover { get; set; }
        public float ComercialVehicle { get; set; }
        public float TeanageDriver { get; set; }
        public float OlderDriver { get; set; }
        public float NightTime { get; set; }
        public float SingleVehicle { get; set; }
        public float DistractedDriving { get; set; }
        public float DrowsyDriving { get; set; }
        public float RoadwayDeparture { get; set; }
        public float Route89 { get; set; }
        public float RouteOther { get; set; }
        public float RoadOther { get; set; }
        public float CityOther { get; set; }
        public float CitySaltLake { get; set; }
        public float CityWestValley { get; set; }
        public float CountyOther { get; set; }
        public float CountySaltLake { get; set; }
        public float CountyUtah { get; set; }
        public float CountyWeber { get; set; }



        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                Milepoint, WorkZone, Pedestrian, Bicyclist, Motorcycle, ImproperRestraint, Unrestrained, DUI, Intersection,
                WildAnimal, DomesticAnimal, Rollover, ComercialVehicle, TeanageDriver, OlderDriver, NightTime, SingleVehicle, 
                DistractedDriving, DrowsyDriving, RoadwayDeparture, Route89, RouteOther, RoadOther, CityOther, CitySaltLake, 
                CityWestValley, CountyOther, CountySaltLake, CountyUtah, CountyWeber
            };
            int[] dimensions = new int[] { 1, 30 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
