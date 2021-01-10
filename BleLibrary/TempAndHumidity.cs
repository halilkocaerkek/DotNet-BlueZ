namespace BleLibrary
{
    public class TempAndHumidity
    {
        public double Temperature { get; }
        public double Humidity { get; }

        public TempAndHumidity(byte[] value)
        {
            Temperature = value[1] + (value[2]/10.0);
            if (value[1] == 1) Temperature *= -1;

            Humidity = value[4] + (value[5]/10.0);
            if (value[3] == 1) Humidity *= -1;
        }
    }
}