namespace Redis.Search.Features.UseCases.GetFunds.Models.v1
{
    public class GetFundsOutput
    {
        public int? Id { get; set; }
        public string? Cnpj { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
