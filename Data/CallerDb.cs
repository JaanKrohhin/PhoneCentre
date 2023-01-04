using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PhoneCentre.Models;
using System;

namespace CallCentreTask.Data
{
    public class CallerDb : DbContext
    {
        public virtual DbSet<Call> Calls { get; set; }
        public virtual DbSet<T_Event> Events { get; set; }
        public virtual DbSet<T_Event_Type> Events_Type { get; set; }

        public static bool DbIsGenerated = false;

        public CallerDb()
        {
            //Checks if the db was generated this startup
            if (!DbIsGenerated)
            {
                DbIsGenerated = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();


                Random rng = new();
                Call currentCall;


                T_Event_Type[] EventTypes = new T_Event_Type[]
                {
                    new T_Event_Type { Event_Id = "EVENT_PICK_UP", Event_Type = "Pick-up", Description = "Generated when user pick ups the phone." },
                    new T_Event_Type { Event_Id = "EVENT_DIAL", Event_Type = "Dialling", Description = "Generated upon the start of the call" },
                    new T_Event_Type { Event_Id = "EVENT_CALL_ESTABLISHED", Event_Type = "Call Established", Description = "Generated when the reciever answers the call." },
                    new T_Event_Type { Event_Id = "EVENT_CALL_END", Event_Type = "Call end", Description = "Generated when one of the party cancels the call, also generated when the reciever just cancels the call." },
                    new T_Event_Type { Event_Id = "EVENT_HANG_UP", Event_Type = "Hang-up", Description = "Generated when user hangs up the phone." },
                };

                Events_Type.AddRange(EventTypes);

                SaveChanges();



                //Initiating counter variables for calls
                int callsTotal = 0;
                int regularCounter = 0;
                int nonDialedCounter = 0;
                int cancelledCounter = 0;



                while (callsTotal < 100)
                {
                    //Randomly chooses how many calls this caller will have, 1-3
                    int numCalls = rng.Next(1, 4);
                    //generates a random phonenumber
                    int caller = RandomAllowedNumber(rng);
                    //generates the calls and events
                    for (int i = 0; i < numCalls; i++)
                    {
                        currentCall = new Call
                        {
                            Caller_ = caller,
                        };


                        //generates a random date and a random time for the call
                        DateTime datetime = RandomDay(rng);

                        //Chooses the call type: regular, cancelled, non-dialled
                        //Only a set number of each type can exist
                        int callType = rng.Next(0, 3);
                        switch (callType)
                        {
                            case 0:
                                if (regularCounter < 80)
                                {

                                    GenerateRegularCalls(rng, currentCall, EventTypes, ref datetime);
                                    regularCounter++;
                                    callsTotal++;

                                }
                                break;


                            case 1:
                                if (nonDialedCounter < 15)
                                {

                                    GenerateNonDialedCalls(rng, currentCall, EventTypes, ref datetime);
                                    nonDialedCounter++;
                                    callsTotal++;

                                }
                                break;

                            case 2:
                                if (cancelledCounter < 5)
                                {

                                    GenerateCancelledCalls(rng, currentCall, EventTypes, ref datetime);
                                    cancelledCounter++;
                                    callsTotal++;

                                }
                                break;
                        }
                    }
                }
                SaveChanges();
            }

            void GenerateRegularCalls(Random rng, Call currentCall, T_Event_Type[] EventTypes, ref DateTime datetime)
            {
                T_Event currenEvent;

                for (int j = 0; j < 5; j++)
                {
                    //Add a receiver and saves the call
                    currentCall.Receiver = RandomAllowedNumber(rng);
                    Calls.Add(currentCall);

                    //Generates the event
                    currenEvent = new T_Event
                    {
                        Event_Type = EventTypes[j],
                        Record_Date = datetime,
                        Call_ = currentCall
                    };
                    Events.Add(currenEvent);

                    //Adds seconds to simulate loading and people talking
                    
                    if (EventTypes[j].Event_Id == "EVENT_CALL_ESTABLISHED")
                    {

                        datetime = datetime.AddSeconds(rng.Next(20, 120));

                    }
                    else
                    {

                        datetime = datetime.AddSeconds(rng.Next(10, 30));

                    }
                }
            }

            void GenerateCancelledCalls(Random rng, Call currentCall, T_Event_Type[] EventTypes, ref DateTime datetime)
            {
                T_Event currenEvent;
                foreach (var event_ in EventTypes)
                {
                    //Add a receiver and saves the call
                    currentCall.Receiver = RandomAllowedNumber(rng);
                    Calls.Add(currentCall);

                        
                    //Adds the needed events 
                    if (event_.Event_Id == "EVENT_PICK_UP" || event_.Event_Id == "EVENT_DIAL" || event_.Event_Id == "EVENT_CALL_END")
                    {
                        currenEvent = new T_Event
                        {
                            Event_Type = event_,
                            Record_Date = datetime,
                            Call_ = currentCall
                        };
                        Events.Add(currenEvent);


                        //Adds seconds to simulate loading
                        datetime = datetime.AddSeconds(rng.Next(5, 30));
                    }
                }
            }
        }

        private void GenerateNonDialedCalls(Random rng, Call currentCall, T_Event_Type[] EventTypes, ref DateTime datetime)
        {
            T_Event currenEvent;
            foreach (var event_ in EventTypes)
            {
                //Saves the call
                Calls.Add(currentCall);


                //Adds the needed events
                if (event_.Event_Id == "EVENT_PICK_UP" || event_.Event_Id == "EVENT_HANG_UP")
                {
                    currenEvent = new T_Event
                    {
                        Event_Type = event_,
                        Record_Date = datetime,
                        Call_ = currentCall
                    };
                    Events.Add(currenEvent);

                    //Adds seconds to simulate loading
                    datetime = datetime.AddSeconds(rng.Next(5, 20));
                }
            }
        }


        //Returns a random phone number that is 6 digits and starts with 3,5 or 8
        int RandomAllowedNumber(Random rng)
        {
            var numberSize = 100000;

            var digits = new int[] { 3, 5, 8 };

            int phoneDigit = digits[rng.Next( 0, digits.Length)];

            return rng.Next(phoneDigit * numberSize, (phoneDigit + 1) * numberSize);
        }

        //Returns a random date with a radom date between 2000/1/1 00:00 and today
        DateTime RandomDay(Random rng)
        {
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;

            start = start.AddDays(rng.Next(range));

            start = start.AddHours(rng.Next(0, 25));
            start = start.AddMinutes(rng.Next(0, 61));
            start = start.AddSeconds(rng.Next(0, 61));

            return start;
        }


        //Creates a connection to the database using the connection string in Resources.resx
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite(PhoneCentre.Properties.Resources.connectionString);
        }
    }
}
