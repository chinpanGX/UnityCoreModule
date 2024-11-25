using System;

namespace Common.Presenter
{
    public interface IPresenter : IDisposable
    {
        void Execute();
    }
}