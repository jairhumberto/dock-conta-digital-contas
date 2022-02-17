using System.ComponentModel.DataAnnotations;

namespace ContasService.Models
{
    public class Portador
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Cpf { get; set; }
    }
}