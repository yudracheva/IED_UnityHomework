using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AllServices
    {
        private readonly List<IService> _services = new();

        public void RegisterSingle<TService>(TService implementation) where TService : IService
        {
            _services.Add(implementation);
        }

        public TService Single<TService>() where TService : IService
        {
            foreach (var t in _services.OfType<TService>()) return t;

            return default;
        }

        public void Cleanup()
        {
            foreach (var t in _services.OfType<ICleanupService>()) t.Cleanup();
        }
    }
}