#nullable enable

namespace Common.Presenter
{
    public class UpdatablePresenter
    {
        private IPresenter? presenter = null;
        private IPresenter? request = null;

        public void Execute()
        {
            if (request != null)
            {
                presenter?.Dispose();
                presenter = request;
                request = null!;
            }

            presenter?.Execute();
        }

        public void SetRequest(IPresenter presenter)
        {
            request = presenter;
        }
    }
}