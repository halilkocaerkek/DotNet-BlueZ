using System.Collections.Generic;

namespace BleLibrary
{
    public class Service : baseUUID
    {
        public Service()
        {
            Characteristics = new List<ICharacteristic>();
        }

        public Service(List<ICharacteristic> characteristics)
        {
            Characteristics = characteristics;
        }

        public List<ICharacteristic> Characteristics { get; set; }
        public Beacon Beacon { get; set; }
    }
}