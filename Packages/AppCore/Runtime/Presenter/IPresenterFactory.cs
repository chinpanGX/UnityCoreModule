using AppCore.Runtime;
using UnityEngine;

namespace App.Title
{
    public interface IPresenterFactory
    {
        Awaitable<IPresenter> CreateAsync();
    }
}