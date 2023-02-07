using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneCentre.Models
{
    [Table("T_CALL")]
    public class Call
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("RECORD_ID")]
        public int Record_Id { get; set; }
        

        [Column("CALLER")]
        public int Caller { get; set; }
        
        
        [Column("RECEIVER")]
        public int Receiver { get; set; }
    }
}
