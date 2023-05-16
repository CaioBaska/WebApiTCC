using System.ComponentModel.DataAnnotations;

namespace API_TCC.Model
{
    public class MonitoramentoModel
    {
        public int Temperatura { get; set; }
        public int Ph { get; set; }
        public int Umidade { get; set; }
        public int Luminosidade { get; set; }
    }

}