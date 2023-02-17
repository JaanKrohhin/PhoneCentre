using MongoDB.Bson;
using MongoDB.Driver;
using PhoneCentre.Models;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace CallCentreTask.Data
{
    public class CallerDb
    {
        private string conn = "mongodb://root:Qwe123!!@192.168.3.68:27017";
        private string DatabaseName = "CallerDB";
        public string Events = nameof(T_Event).ToUpper();
        public string TypesOfEvents = nameof(T_Event_Type).ToUpper();
        public string Calls = nameof(Call).ToUpper();

        //Made a T generic function first but it only generated 1 table named "T"
        public IMongoCollection<T_Event> GetEventTable()
        {
            var client = new MongoClient(conn);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T_Event>(Events);
        }
        public IMongoCollection<T_Event_Type> GetTypeTable()
        {
            var client = new MongoClient(conn);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T_Event_Type>(TypesOfEvents);
        }
        public IMongoCollection<Call> GetCallTable()
        {
            var client = new MongoClient(conn);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<Call>(Calls);
        }

        public void InsertOneToMongoTable(Call ModelItem)
        {
            GetCallTable().InsertOne(ModelItem);
        }
        public void InsertOneToMongoTable(T_Event ModelItem)
        {
            GetEventTable().InsertOne(ModelItem);
        }
        public void InsertManyToMongoTable(T_Event_Type[] ModelItem)
        {
            GetTypeTable().InsertMany(ModelItem);
        }
        private void ClearAllCollections()
        {
            var client = new MongoClient(conn);
            var db = client.GetDatabase(DatabaseName);
            db.DropCollection("T");
            db.DropCollection(Events);
            db.DropCollection(Calls);
            db.DropCollection(TypesOfEvents);
        }
        public static bool DbIsGenerated = false;

        public CallerDb()
        {
            //Checks if the db was generated this startup
            if (!DbIsGenerated)
            {
                DbIsGenerated = true;
                ClearAllCollections();

                Random rng = new();


                Call currentCall;


                T_Event_Type[] EventTypes = new T_Event_Type[]
                {
                    new T_Event_Type { Event_Id = "EVENT_PICK_UP", Event_Type = "Pick-up", Description = "Generated when user pick ups the phone." },
                    new T_Event_Type { Event_Id = "EVENT_DIAL", Event_Type = "Dialling", Description = "Generated upon the start of the call" },
                    new T_Event_Type { Event_Id = "EVENT_CALL_ESTABLISHED", Event_Type = "Call Established", Description = "Generated when the reciever answers the call." },
                    new T_Event_Type { Event_Id = "EVENT_CALL_END", Event_Type = "Call End", Description = "Generated when one of the party cancels the call, also generated when the reciever just cancels the call." },
                    new T_Event_Type { Event_Id = "EVENT_HANG_UP", Event_Type = "Hang-up", Description = "Generated when user hangs up the phone." },
                };

                InsertManyToMongoTable(EventTypes);

               



                //Initiating counter variables for calls
                int callsTotal = 1;
                int regularCounter = 0;
                int nonDialedCounter = 0;
                int cancelledCounter = 0;


                while (callsTotal < 101)
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
                            Caller = caller,
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
                Console.WriteLine("Database generated");
               

            }

            void GenerateRegularCalls(Random rng, Call currentCall, T_Event_Type[] EventTypes, ref DateTime datetime)
            {
                T_Event currenEvent;
                for (int j = 0; j < 5; j++)
                {
                    //Add a receiver and saves the call
                    currentCall.Receiver = RandomAllowedNumber(rng);
                    InsertOneToMongoTable(currentCall);

                    //Generates the event
                    currenEvent = new T_Event
                    {
                        Event_Type = EventTypes[j],
                        Record_Date = datetime,
                        Call_ = currentCall
                    };
                    InsertOneToMongoTable(currenEvent);

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
                    InsertOneToMongoTable(currentCall);

                        
                    //Adds the needed events 
                    if (event_.Event_Id == "EVENT_PICK_UP" || event_.Event_Id == "EVENT_DIAL" || event_.Event_Id == "EVENT_CALL_END")
                    {
                        currenEvent = new T_Event
                        {
                            Event_Type = event_,
                            Record_Date = datetime,
                            Call_ = currentCall
                        };
                        Console.WriteLine(currenEvent.FormatToCvsString());
                        InsertOneToMongoTable(currenEvent);


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
                InsertOneToMongoTable(currentCall);


                //Adds the needed events
                if (event_.Event_Id == "EVENT_PICK_UP" || event_.Event_Id == "EVENT_HANG_UP")
                {
                    currenEvent = new T_Event
                    {
                        Event_Type = event_,
                        Record_Date = datetime,
                        Call_ = currentCall
                    };
                    InsertOneToMongoTable(currenEvent);

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
    }
}
