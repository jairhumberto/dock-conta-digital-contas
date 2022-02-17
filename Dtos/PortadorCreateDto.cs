using System.ComponentModel.DataAnnotations;

namespace ContasService.Dtos
{
    public class PortadorCreateDto
    {
        [Required]
        public string Cpf { get; set; }
    }
}