using System.Collections.Generic;

namespace App.Title
{
    public sealed class PresenterFactoryProvider
    {
        private static readonly Dictionary<string, IPresenterFactory> PresenterFactory = new();

        public void TryAdd(string key, IPresenterFactory factory)
        {
            PresenterFactory.TryAdd(key, factory);
        }
        
        public IPresenterFactory Get(string name)
        {
            return PresenterFactory[name];
        }
    }
}