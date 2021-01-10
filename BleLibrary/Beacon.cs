using System.Collections.Generic;

namespace BleLibrary
{
    public class Beacon : baseUUID
    {
        public Beacon()
        {
            Services = new List<Service>();
        }

        public Beacon(List<Service> services)
        {
            Services = services;
        }

        public List<Service> Services { get; set; }
    }
}