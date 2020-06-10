using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Excersice
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> drivers = new List<string>();
            List<Trip> trips = new List<Trip>();
            string command;
            do
            {
                command = System.Console.ReadLine();
                StringBuilder errors = new StringBuilder();


                if (command.Length >= 4 && command.Substring(0, 4) == "Trip")
                {
                    var arguments = command.Substring(4).Trim().Split(' ');
                    if (arguments.Count() == 4)
                    {
                        var driver = arguments[0];
                        var initial = arguments[1];
                        var final = arguments[2];
                        var miles = arguments[3];
                        TimeSpan tsInitial = new TimeSpan();
                        TimeSpan tsFinal = new TimeSpan();
                        decimal fMiles;
                        Regex regExp = new Regex("([0-1][0-9]|2[0-3]):[0-5][0-9]");

                        if (!drivers.Contains(driver))
                        {
                            errors.AppendLine("The Driver is not registered");
                        }
                        if (!regExp.IsMatch(initial))
                        {
                            errors.AppendLine("Initial Time incorrect format. Correct format 00:00 in hours and minutes");
                        }
                        else
                        {
                            tsInitial = TimeSpan.Parse(initial);
                        }
                        if (!regExp.IsMatch(final))
                        {
                            errors.AppendLine("Final Time incorrect format. Correct format 00:00 in hours and minutes");
                        }
                        else
                        {
                            tsFinal = TimeSpan.Parse(final);
                        }
                        if (tsInitial >= tsFinal)
                        {
                            errors.AppendLine("The initial time cannot be greater than te final time");
                        }
                        if (!decimal.TryParse(miles, out fMiles))
                        {
                            errors.AppendLine("The Miles input is incorrect.");
                        }
                        else
                        {
                            fMiles = Math.Round(fMiles, 0);
                        }

                        if (errors.Length == 0)
                        {
                            var averageSpeed = Math.Round((60 * fMiles) / (decimal)(tsFinal - tsInitial).TotalMinutes, 0);
                            if (averageSpeed > 5 && averageSpeed < 100)
                            {
                                Trip trip = new Trip();
                                trip.Driver = driver;
                                trip.Initial = tsInitial;
                                trip.Final = tsFinal;
                                trip.Miles = fMiles;
                                trip.AveregeSpeed = averageSpeed;

                                trips.Add(trip);
                            }

                        }
                    }
                    else
                    {
                        errors.AppendLine("Trip Command incomplete. Example:\"Trip Dan 07:15 07:45 17.3\".");
                    }

                }
                if (command.Length >= 6 && command.Substring(0, 6) == "Driver")
                {
                    var driver = command.Substring(6).Trim();
                    if (driver != "")
                    {

                        drivers.Add(driver);

                    }
                    else
                    {
                        errors.AppendLine("You have to input a driver. Example : \"Driver Name\"");
                    }
                }

                if (errors.Length > 0)
                {
                    System.Console.WriteLine(errors);
                }

            } while (command != "exit");

            List<Output> report = new List<Output>();
            foreach(var d in drivers)
            {
                Output outPut = new Output();
                outPut.Driver = d;
                if (trips.Where(w => w.Driver == d).Any())
                {
                    outPut.MilesDriven = trips.Where(w => w.Driver == d).Select(s => s.Miles).Sum();
                    outPut.AvgSpeed = trips.Where(w => w.Driver == d).Select(s => s.AveregeSpeed).Average();
                }
                report.Add(outPut);
                
            }

            var  reportOutput= report.OrderByDescending(o => o.MilesDriven);

            foreach(var r in reportOutput)
            {
                System.Console.WriteLine(r.Driver + ": " + r.MilesDriven + " Miles @ " + r.AvgSpeed + " mph");
            }

            System.Console.ReadLine();
        }

        public struct Trip
        {
            public TimeSpan Initial { get; set; }
            public TimeSpan Final { get; set; }
            public decimal Miles { get; set; }
            public string Driver { get; set; }
            public decimal AveregeSpeed { get; set; }
        }

       

        public struct Output
        {
            public string Driver { get; set; }
            public decimal MilesDriven { get; set; }
            public decimal AvgSpeed { get; set; }
        }
    }
}
