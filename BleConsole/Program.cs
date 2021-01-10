using System;
using System.Linq;
using System.Threading.Tasks;
using BleLibrary;

namespace BleConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var adapter = new Adapter();
            await adapter.init();

            //var Beacon = new Beacon(adapter,"ED:90:BA:7E:B5:14");
            var Beacon = new Beacon(adapter,"70:F1:00:00:0A:20");

            await Beacon.Init();

            Beacon.ServiceIds.ToList().ForEach(Console.WriteLine);

            var result = await Beacon.GetTempAndHumidity(serviceName : "0000aa20-0000-1000-8000-00805f9b34fb", characteristicName : "0000aa21-0000-1000-8000-00805f9b34fb");
            if (result != null)
            {
                for (var i = 0; i < 10; i++)
                {
                    Console.Write($"{DateTime.Now} ");
                    Console.Write($"Temperature is : {result.Temperature} ");
                    Console.WriteLine($",Humidity is : {result.Humidity} ");
                    await Task.Delay(1000);
                }
            }
        }
    }
}