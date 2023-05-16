using System.ComponentModel.DataAnnotations;

namespace API_TCC.Model
{
    public class UsuarioModel
    {
        [Key]
        public string ?Login { get; set; }
        public string ?Senha { get; set; }
    }

}