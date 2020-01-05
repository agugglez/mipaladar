using System;
using System.Collections.Generic;

namespace MiPaladar.MVVM
{
    public class ServiceContainer
    {
        public static readonly ServiceContainer Instance = new ServiceContainer();

        private ServiceContainer()
        {
            //_serviceMap = new Dictionary<Type, object>();
            //_serviceMapLock = new object();
        }

        public static void AddService<TServiceContract>(TServiceContract implementation)
            where TServiceContract : class
        {
            lock (_serviceMapLock)
            {
                _serviceMap[typeof(TServiceContract)] = implementation;
            }
        }

        public static TServiceContract GetService<TServiceContract>()
            where TServiceContract : class
        {
            object service;
            lock (_serviceMapLock)
            {
                _serviceMap.TryGetValue(typeof(TServiceContract), out service);
            }
            return service as TServiceContract;
        }

        static readonly Dictionary<Type, object> _serviceMap = new Dictionary<Type, object>();
        static readonly object _serviceMapLock = new object();
    }
}