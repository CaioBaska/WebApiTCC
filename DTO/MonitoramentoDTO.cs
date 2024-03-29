﻿namespace API_TCC.DTO
{
    public class MonitoramentoDTO
    {
        public DateTime DATA { get; set; }
        public string? UMIDADE { get; set; }
        public string? TEMPERATURA { get; set; }
        public string? POTASSIO { get; set; }
        public string? PH { get; set; }
        public string? NITROGENIO { get; set; }

        public string? FOSFORO { get; set; } 

        public string? LUMINOSIDADE { get; set; }

        public string DataFormatada { get; set; }
    }
    public class RelatorioDTO
    {
        public string DataFormatada { get; set; }
        public string? UMIDADE { get; set; }
        public string? TEMPERATURA { get; set; }
        public string? POTASSIO { get; set; }
        public string? PH { get; set; }
        public string? NITROGENIO { get; set; }

        public string? FOSFORO { get; set; }

        public string? LUMINOSIDADE { get; set; }

    }
}
