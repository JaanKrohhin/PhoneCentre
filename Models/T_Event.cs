using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class T_Event
    {
        [BsonElement("RECORD_ID")]
        public int Record_Id { get; set; }

        [BsonElement("RECORD_EVENT_ID")]
        public string Record_Event_Id { get => Event_Type.Event_Id; }

        [BsonElement("EVENT_TYPE")]
        public T_Event_Type Event_Type { get; set; }

        [BsonElement("RECORD_DATE")]
        public DateTime Record_Date { get; set; }

        [BsonElement("CALL_ID")]
        public int? Call_Id { get => Call_.Record_Id; }

        [BsonElement("CALL")]
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