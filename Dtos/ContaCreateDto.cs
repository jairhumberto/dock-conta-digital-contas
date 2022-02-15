using System.ComponentModel.DataAnnotations;

namespace ContasService.Dtos
{
    public class ContaCreateDto
    {
        [Required]
        public string Agencia { get; set; }

        [Required]
        public string PortadorCpf { get; set; }
    }
}