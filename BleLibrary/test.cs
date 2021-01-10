namespace BleLibrary
{
    public class test
    {
        public void init()
        {

            var b = new Beacon();
            var s = new Service();
            var c = new Characteristic();
            s.Characteristics.Add(c);
            b.Services.Add(s);
        }
    }
}