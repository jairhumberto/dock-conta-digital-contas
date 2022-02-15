namespace ContasService.Dtos
{
    public class ContaReadDto
    {
        public string Numero { get; set; }
        public string Agencia { get; set; }
        public decimal Saldo { get; set; }
        public string PortadorCpf { get; set; }
        public bool Ativa { get; set; }
        public bool Bloqueada { get; set; }
    }
}