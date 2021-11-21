using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIConsume.Models
{
    public class User
    {
        public int m_dukcapil_data_id { get; set; }
        public string NIK { get; set; }
        public string name { get; set; }
        public string maiden_name { get; set; }
        public DateTime birth_date { get; set; }
        public string gender { get; set; }
        public int religion_id { get; set; }
        public int marital_status_id { get; set; }
    }
}