using System;
using UI.Base;
using UnityEngine;

namespace Services.UI.Factory
{
    public interface IUIFactory : IService
    {
        event Action<WindowId, BaseWindow> Spawned;
        void CreateWindow(WindowId id);
        GameObject CreateUIRoot();
    }
}