using Redis.OM.Modeling;

namespace Redis.Search.Features.UseCases.GetFunds.Models.v1
{
    public class GetFundsOutput
    {
        [RedisIdField]
        public int? id { get; set; }
        public string? cnpj { get; set; }
        public string? name { get; set; }
        public string? @class { get; set; }
    }
}
