using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class T_Event
    {
        public ObjectId _id { get; set; }
        public int Record_Id { get; set; }

        public string? Record_Event_Id { get; set; }

        public T_Event_Type? Event_Type { get; set; }

        public DateTime Record_Date { get; set; }

        public int? Call_Id { get; set; }

        public Call? Call_ { get; set; }

        public string FormatToCvsString()
        {
            string caller = Call_.Caller.ToString();
            string receiver = Call_.Receiver.ToString();
            string event_ = Event_Type.Event_Type.Trim();
            string date = Record_Date.ToString("dd-MM-yyyy HH:mm:ss");
            return $"{caller},{event_},{receiver},{date}";
        }
    }
}