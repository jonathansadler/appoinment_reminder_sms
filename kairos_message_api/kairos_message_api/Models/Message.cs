using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

namespace kairos_message_api.Models
{
    public class Message
    {

        [Key]
        public int message_id { get; set; }
        public Guid message_unique_id { get; set; }

        public int message_job_type { get; set; }

        public DateTime time_for_delivery_utc { get; set; }

        public int client_id { get; set; }

        public String client_name { get; set; }

        public String message_body { get; set; }

        //[System.ComponentModel.DefaultValue("8765507295")]
        public String sms_phone_no { get; set; }



    }
}
