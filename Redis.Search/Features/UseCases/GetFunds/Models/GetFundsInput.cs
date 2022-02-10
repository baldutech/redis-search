using MediatR;
using System.Collections.Generic;

namespace Redis.Search.Features.UseCases.GetFunds.Models
{
    public class GetFundsInput : IRequest<IEnumerable<GetFundsOutput>>
    {
        public int? Id { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string? Select { get; set; }

        internal string[] RedisQueryFormatted =>
            new RedisQuery("fund-index")
            {
                QueryText = GetQueryTextFormatted(),
                Limit = new SearchLimit(Limit, Offset),
                Return = new ReturnFields(Select?.Split(","))

            }.ToArray();


        public bool IsValid() =>
            !string.IsNullOrEmpty(Select);

        public string? GetQueryTextFormatted()
        {
            if (Id.HasValue && Id.Value > 0)
            {
                return $"@id:{{{Id}}}";
            }

            return null;
        }
    }
}
