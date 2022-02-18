using MediatR;
using Redis.Search.Features.UseCases.GetFunds.Models;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Features.UseCases.GetFunds.UseCase
{
    public class CreateFundsCacheUseCase : IRequestHandler<CreateFundsCacheInput, bool>
    {
        private readonly IDatabase _database;

        public CreateFundsCacheUseCase(
            IDatabase database)
        {
            _database = database;
        }

        public async Task<bool> Handle(CreateFundsCacheInput request, CancellationToken cancellationToken)
        {
            

            return true;
        }
    }
}
