using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class T_Event_Type
    {
        public string Event_Id { get; set; }
        public string Event_Type { get; set; }
        public string Description { get; set; }
    }
}
