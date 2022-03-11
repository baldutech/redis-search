using MediatR;
using Redis.Search.Features.UseCases.GetFunds.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Features.UseCases.GetFunds.UseCase.v2
{
    public class GetFundsUseCase : IRequestHandler<GetFundsInput, IEnumerable<GetFundsOutput>>
    {
        private readonly IDatabase _database;

        public GetFundsUseCase(
            IDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<GetFundsOutput>> Handle(GetFundsInput request, CancellationToken cancellationToken)
        {
            var redisResults = await _database.ExecuteAsync("FT.SEARCH", request.RedisQueryFormatted);

            return ((RedisResult[])redisResults).Where(x => x.Type == ResultType.MultiBulk).Select(redisResult =>
            {
                var redisResultIntern = redisResult.ToDictionary();

                redisResultIntern.TryGetValue("cnpj", out RedisResult? cnpj);
                redisResultIntern.TryGetValue("name", out RedisResult? name);
                redisResultIntern.TryGetValue("fundId", out RedisResult? fundId);
                redisResultIntern.TryGetValue("isActive", out RedisResult? isActive);
                
                return new GetFundsOutput
                {
                    Id = ((int?)fundId),
                    Cnpj = ((string)cnpj),
                    Name = ((string)name),
                    IsActive = ((string)isActive) == null ? null : bool.Parse(((string)isActive))
                };
            });
        }
    }
}
