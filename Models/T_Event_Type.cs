using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class T_Event_Type
    {
        [BsonElement("EVENT_ID")]
        public string Event_Id { get; set; }

        [BsonElement("EVENT_NAME")]
        public string Event_Type { get; set; }

        [BsonElement("DESCRIPTION")]
        public string Description { get; set; }
    }
}
