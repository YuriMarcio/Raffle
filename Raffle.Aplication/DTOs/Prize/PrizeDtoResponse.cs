namespace Raffle.Aplication.DTOs.Prize
{
    public class PrizeDtoResponse
    {
        // Propriedade privada para armazenar o objeto Prize
        private readonly Domain.Entities.Prize _prize;

        // Construtor que recebe um objeto Prize
        public PrizeDtoResponse(Domain.Entities.Prize prize)
        {
            _prize = prize;
            Id = _prize.Id;
            Title = _prize.Title;
            RaffleId = _prize.RaffleId;
            // Caso precise de mais propriedades, você pode adicionar aqui.
        }

        // Propriedades públicas para retorno dos dados
        public string Id { get; set; }
        public string Title { get; set; }
        public string RaffleId { get; set; }
    }
}
