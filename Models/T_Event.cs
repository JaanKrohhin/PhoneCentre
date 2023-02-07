using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneCentre.Models
{
    [Table("T_EVENT")]
    public class T_Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("RECORD_ID")]
        public int Record_Id { get; set; }

        [Column("RECORD_EVENT_ID")]
        public string Record_Event_Id { get; set; }
        [ForeignKey("Record_Event_Id")]
        public T_Event_Type Event_Type { get; set; }

        [Column("RECORD_DATE"), DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime Record_Date { get; set; }

        [Column("CALL_ID")]
        [ForeignKey("Call_")]
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