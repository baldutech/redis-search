using MediatR;
using System.Collections.Generic;

namespace Redis.Search.Features.UseCases.GetFunds.Models.v1
{
    public class GetFundsInput : IRequest<IEnumerable<GetFundsOutput>>
    {
        public int? Id { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string QueryText { get; set; } = "*";
        public string Select { get; set; } = string.Empty;

        public bool IsValid() =>
            !string.IsNullOrEmpty(Select);
    }
}
