using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

namespace BleLibrary
{
    public class Beacon
    {
        private string deviceAddress { get; set; }
        static TimeSpan timeout = TimeSpan.FromSeconds(15);
        private Device device { get; set; }
    

    private readonly Adapter _adapter;
        public List<Service> Services { get; private set; }
        public string[] ServiceIds { get; set; }

        public Beacon(Adapter adapter, string deviceAddress)
        {
            _adapter = adapter;
            this.deviceAddress = deviceAddress;
            Services = new List<Service>();
        }

        public Beacon AddServices(Service service)
        {
            service.Beacon = this;
            Services.Add(service);
            return this;
        }

        public async Task<bool> Init()
        {
            device = await _adapter.Instance.GetDeviceAsync(deviceAddress);
            if (device == null)
            {
                Console.WriteLine($"Bluetooth peripheral with address '{deviceAddress}' not found. Use `bluetoothctl` or Bluetooth Manager to scan and possibly pair first.");
                return false;
            }
            Console.WriteLine("Connecting...");
            await device.ConnectAsync();
            await device.WaitForPropertyValueAsync("Connected", value: true, timeout);
            Console.WriteLine("Connected.");

            Console.WriteLine("Waiting for services to resolve...");
            await device.WaitForPropertyValueAsync("ServicesResolved", value: true, timeout);

            ServiceIds = await device.GetUUIDsAsync();
            Console.WriteLine($"Device offers {ServiceIds.Length} service(s).");
            
            return true;
        }

        public List<Service> GetServices()
        {
            ServiceIds.ToList().ForEach(async s =>
            {
           
                var service = await device.GetServiceAsync(s);
                if (service != null)
                {
                    var chars = await service.GetAllCharacteristicAsync();
                    chars?.ToList()
                        .ForEach(async c =>
                        {
                            var characteristic = await service.GetCharacteristicAsync(c);
                            try
                            {
                                byte[] value = await characteristic.ReadValueAsync(timeout);
                                Console.WriteLine($"Service: {s}, characteristic: {c}");
                                Console.WriteLine($"\tValue: {value.ToHex()}, {value.ToInt()}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Service: {s}, characteristic: {c}");
                                Console.WriteLine($"\t{c} - Value couldn't read: {ex.Message}");
                            }
                        });
                }
                else
                {
                    Console.WriteLine("Service is null");
                }
            });

            return Services;
        }

        public async Task<TempAndHumidity> GetTempAndHumidity(string serviceName, string characteristicName)
        {
            var service = await device.GetServiceAsync(serviceName);
            if (service != null)
            {
                var characteristic = await service.GetCharacteristicAsync(characteristicName);
                try
                {
                    byte[] value = await characteristic.ReadValueAsync(timeout);
                    Console.WriteLine($"Service: {serviceName}, characteristic: {characteristicName}");
                    Console.WriteLine($"\tValue: {value.ToHex()}, {value.ToInt()}");
                   
                    return new TempAndHumidity(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Service: {serviceName}, characteristic: {characteristicName}");
                    Console.WriteLine($"\t{characteristicName} - Value couldn't read: {ex.Message}");
                }
            }

            return null;
        }
    }

    public class Adapter
    {
        private string adapterId { get; set; }
        public IAdapter1 Instance { get; set; } 
        
        public async Task<bool> init()
        {
            if (!string.IsNullOrEmpty(adapterId))
            {
                Instance = await BlueZManager.GetAdapterAsync(adapterId);
                return true;
            }
            else
            {
                var adapters = await BlueZManager.GetAdaptersAsync();
                if (adapters.Count == 0)
                {
                    throw new Exception("No Bluetooth adapters found.");
                }

                Instance = adapters.First();
            }
            
            var adapterPath = Instance.ObjectPath.ToString();
            var adapterName = adapterPath.Substring(adapterPath.LastIndexOf("/") + 1);
            Console.WriteLine($"Using Bluetooth adapter {adapterName}");
            return true;
        }
    }
}