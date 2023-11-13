﻿using System.ComponentModel.DataAnnotations;

namespace API_TCC.Model
{
    public class MonitoramentoModel
    {
        [Key]
        public int ID { get; set; }
        public int TEMPERATURA { get; set; }
        public int PH { get; set; }
        public int UMIDADE { get; set; }
        public int LUMINOSIDADE { get; set; }
    }
}
