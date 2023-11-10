using QlmLicenseLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace StressTestGeneratedIds
{
    class Program
    {
        private static readonly string smBiosUUID;
        private static readonly string machineName;
        private static readonly string uniqueSystemIdentifier;
        private static readonly string volumeSerial;
        private static readonly bool runningOnVM;

        static Program()
        {
            smBiosUUID = GetSMBiosUUID();
            machineName = GetMachineName();
            uniqueSystemIdentifier = GetUniqueSystemIdentifier1();
            volumeSerial = GetVolumeSerial();
            runningOnVM = RunningOnVM();
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"RunningOnVM: {RunningOnVM()}");
            Console.WriteLine($"GetSMBiosUUID: {GetSMBiosUUID()}");
            Console.WriteLine($"GetMachineName: {GetMachineName()}");
            Console.WriteLine($"GetUniqueSystemIdentifier1: {GetUniqueSystemIdentifier1()}");
            Console.WriteLine($"GetVolumeSerial: {GetVolumeSerial()}");
            Console.ReadLine();

            var tasks = new Task[16];
            for (var i = 0; i < 16; i++)
                tasks[i] = Task.Run(StressTest);
            Task.WaitAll(tasks);
        }

        static void StressTest()
        {
            while (true)
                if (GetSMBiosUUID() != smBiosUUID ||
                    GetMachineName() != machineName ||
                    GetUniqueSystemIdentifier1() != uniqueSystemIdentifier ||
                    GetVolumeSerial() != volumeSerial ||
                    RunningOnVM() != runningOnVM)
                    Console.WriteLine("One or more values have changed during execution!");
        }

        static bool RunningOnVM() => new QlmHardware().RunningOnVM();

        static string GetSMBiosUUID() => new QlmHardware().GetSMBiosUUID();

        static string GetMachineName() => new QlmHardware().GetMachineName();

        static string GetUniqueSystemIdentifier1() => new QlmHardware().GetUniqueSystemIdentifier1();

        static string GetVolumeSerial()
        {
            var disk = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
            disk.Get();
            var volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();
            return volumeSerial;
        }
        
    }
}
