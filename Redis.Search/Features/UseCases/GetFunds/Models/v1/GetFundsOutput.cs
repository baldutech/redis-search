using Redis.OM.Modeling;

namespace Redis.Search.Features.UseCases.GetFunds.Models.v1
{
    public class GetFundsOutput
    {
        [RedisIdField]
        public int? id { get; set; }

        [Indexed]
        public string? cnpj { get; set; }

        [Indexed]
        public string? name { get; set; }

        [Indexed]
        public bool? isActive { get; set; }
    }
}
