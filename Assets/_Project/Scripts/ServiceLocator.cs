using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts
{
    public class ServiceLocator
    {
        public static readonly ServiceLocator Global = new ServiceLocator();

        private readonly Dictionary<int, object> _registeredServices = new Dictionary<int, object>();

        public TService GetService<TService>()
        {
            int serviceHash = typeof(TService).GetHashCode();

            if (_registeredServices.TryGetValue(serviceHash, out object service))
            {
                return (TService)service;
            }

            return default;
        }

        /// <summary>
        /// Registers a service. Type registered needs to be an interface, otherwise will throw a
        /// <see cref="NonInterfaceServiceException"/>.
        /// </summary>
        public void RegisterService<TService>(TService service)
        {
            Type serviceType = typeof(TService);

            if (!serviceType.IsInterface)
            {
                throw new NonInterfaceServiceException(serviceType);
            }

            int serviceHash = serviceType.GetHashCode();

            if (_registeredServices.ContainsKey(serviceHash))
            {
                throw new ServiceAlreadyRegisteredException(serviceType);
            }

            _registeredServices.Add(serviceType.GetHashCode(), service);
        }

        public bool UnregisterService<TService>()
        {
            Type serviceType = typeof(TService);
            int serviceHash = serviceType.GetHashCode();

            if (_registeredServices.ContainsKey(serviceHash))
            {
                _registeredServices.Remove(serviceHash);

                return true;
            }

            return false;
        }

        public void ClearServices()
        {
            _registeredServices.Clear();
        }

        public IEnumerable<object> GetServices()
        {
            return _registeredServices.Select(x => x.Value);
        }
    }

    public class NonInterfaceServiceException : Exception
    {
        private Type _invalidType;

        public NonInterfaceServiceException(Type invalidType)
        {
            _invalidType = invalidType;
        }

        public override string Message => $"Attempted to register '{_invalidType.Name}' as a service. Only interfaces can be registered as services";
    }

    public class ServiceAlreadyRegisteredException : Exception
    {
        private Type _serviceType;

        public ServiceAlreadyRegisteredException(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public override string Message => $"Attempted to register service '{_serviceType.Name}', but it's already been registered";
    }
}