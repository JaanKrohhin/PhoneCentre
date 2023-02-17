using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class Call
    {
        [BsonElement("RECORD_ID")]
        public int Record_Id { get; set; }

        [BsonElement("CALLER")]
        public int Caller { get; set; }
        
        
        [BsonElement("RECEIVER")]
        public int Receiver { get; set; }
    }
}
