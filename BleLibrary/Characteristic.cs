namespace BleLibrary
{
    public interface ICharacteristic
    {
        public string UUID { get; set; } 
        public string Name { get; set; }


        public object getValue();
    }

    public class Temperature : ICharacteristic
    {
        public string UUID { get; set; }
        public string Name { get; set; }

        public Temperature(string name, string uuid)
        {
            Name = name;
            UUID = uuid;
        }

        public object getValue()
        {
            return null;
        }
    }
}