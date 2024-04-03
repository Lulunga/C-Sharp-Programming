using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public class Device
    {
        public int DeviceId { get; }
        public string Name { get; }
        public List<Failure> Failures { get; }

        public Device(int deviceId, string name, List<Failure> failures)
        {
            DeviceId = deviceId;
            Name = name;
            Failures = failures;
        }
    }

    public class Failure
    {
        public FailureType Type { get; }
        public DateTime Date { get; }

        public Failure(FailureType type, DateTime date) => (Type, Date) = (type, date);

        public bool IsSerious() => Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailures;
    }

    public class Common
    {
        public static bool IsEarlier(DateTime date1, DateTime date2) => date1 < date2;
    }

    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceIds,
            object[][] dates,
            List<Dictionary<string, object>> devicesList)
        {
            var devices = new Device[deviceIds.Length];
            for (var i = 0; i < deviceIds.Length; i++)
            {
                var deviceId = (int)devicesList[i]["DeviceId"];
                var name = (string)devicesList[i]["Name"];
                var failureDate = new DateTime((int)dates[i][2], (int)dates[i][1], (int)dates[i][0]);
                var failure = new Failure((FailureType)failureTypes[i], failureDate);

                List<Failure> failures = new List<Failure>();
                failures.Add(failure);

                devices[i] = new Device(deviceId, name, failures);
            }
            var date = new DateTime(year, month, day);
            return FindDevicesFailedBeforeDate(date, devices);
        }

        public static List<string> FindDevicesFailedBeforeDate(DateTime date, Device[] devices)
        {
            var problematicDevices = devices
                 .Where(d => d.Failures.Any(f => f.IsSerious() && Common.IsEarlier(f.Date, date)))
                 .Select(d => d.DeviceId)
                 .ToHashSet();

            return devices
                .Where(device => problematicDevices.Contains(device.DeviceId))
                .Select(device => device.Name)
                .ToList();
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown,
        NonResponding,
        HardwareFailures,
        ConnectionProblems
    }
}