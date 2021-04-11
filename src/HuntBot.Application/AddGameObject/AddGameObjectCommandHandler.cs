using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace HuntBot.Application.AddGameObject
{
    public class AddGameObjectCommandHandler : IRequestHandler<AddGameObjectCommand>
    {
        public AddGameObjectCommandHandler()
        {
            
        }

        public Task<Unit> Handle(AddGameObjectCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}