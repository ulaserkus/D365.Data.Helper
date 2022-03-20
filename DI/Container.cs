using D365.Data.Helper.CRM;
using System;
using System.Collections.Generic;

namespace D365.Data.Helper
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> regs = new Dictionary<Type, Func<object>>();

        public void Register<TService, TImpl>(object Info) where TImpl : TService =>
            regs.Add(typeof(TService), () => this.GetInstance(typeof(TImpl),Info));

        public bool HasKey(Type type)
        {
            return regs.ContainsKey(type);
        }

        public void Register<TService>(Func<TService> factory) =>
       regs.Add(typeof(TService), () => factory());

        public void RegisterInstance<TService>(TService instance) =>
            regs.Add(typeof(TService), () => instance);

        public void RegisterSingleton<TService>(Func<TService> factory)
        {
            var lazy = new Lazy<TService>(factory);
            Register(() => lazy.Value);
        }

        public object GetInstance(Type type, object Info)
        {
            if (regs.TryGetValue(type, out Func<object> Fac)) return Fac();
            else if (!type.IsAbstract) return CreateInstance(type, Info);
            throw new InvalidOperationException("No registration for " + type);
        }

        private object CreateInstance(Type implementationType, object Info)
        {
            return Activator.CreateInstance(implementationType, Info);
        }
    
    }
}
