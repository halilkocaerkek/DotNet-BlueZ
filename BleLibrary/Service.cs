using System.Collections.Generic;

namespace BleLibrary
{
    public class Service : baseUUID
    {
        public Service()
        {
            Characteristics = new List<Characteristic>();
        }

        public Service(List<Characteristic> characteristics)
        {
            Characteristics = characteristics;
        }

        public List<Characteristic> Characteristics { get; set; }

    }
}