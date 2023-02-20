using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneCentre.Models
{
    public class Call
    {
        public int Record_Id { get; set; }
        public int Caller { get; set; }
        public int Receiver { get; set; }
    }
}
