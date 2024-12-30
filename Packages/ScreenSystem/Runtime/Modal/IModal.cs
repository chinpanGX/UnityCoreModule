using System.Threading;
using Cysharp.Threading.Tasks;

namespace ScreenSystem.Modal
{
    public interface IModal
    {

        string ModalId { get; }
        UniTask OnCompleteAsync(CancellationToken cancellationToken);
    }
}