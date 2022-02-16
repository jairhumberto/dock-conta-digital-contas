using System.ComponentModel.DataAnnotations;

namespace ContasService.Dtos
{
    public class ContaUpdateDto
    {
        [Required]
        public bool Bloqueada { get; set; }

        [Required]
        public bool Ativa { get; set; }
    }
}