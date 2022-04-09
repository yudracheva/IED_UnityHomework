﻿using System.Collections.Generic;
using System.Linq;

namespace Services
{
  public class AllServices
  {
    private readonly List<IService> _services = new List<IService>();
    public void RegisterSingle<TService>(TService implementation) where TService : IService =>
      _services.Add(implementation);

    public TService Single<TService>() where TService : IService
    {
      foreach (var t in _services.OfType<TService>())
      {
        return t;
      }

      return default;
    }

    public void Cleanup()
    {
      for (int i = 0; i < _services.Count; i++)
      {
        if (_services[i] is ICleanupService)
          ((ICleanupService)_services[i]).Cleanup();
      }
    }
  }
}