using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneCentre.Models
{
    [Table("T_EVENT_TYPE")]
    public class T_Event_Type
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("EVENT_ID")]
        public string Event_Id { get; set; }
        [Column("EVENT_NAME")]
        public string Event_Type { get; set; }
        public string Description { get; set; }
    }
}
