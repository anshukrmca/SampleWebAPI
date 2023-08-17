using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.Infrastructure.Services
{
    public interface IService
    {
    }
    public sealed class ServiceManager
    {
        private static object _thisLock = new Object();
        private static Dictionary<Type, IService> _serviceInstances = new Dictionary<Type, IService>();
        public static T GetService<T>() where T : IService
        {
            lock (_thisLock)
            {
                if (!_serviceInstances.ContainsKey(typeof(T)))
                    _serviceInstances.Add(typeof(T), GetInstance<T>());
            }
            return (T)_serviceInstances[typeof(T)];
        }

        private static IService GetInstance<T>(string connection = "")
        {
            IService obj = null;
            switch (typeof(T).Name)
            {
                case "IEmpService":
                    obj = string.IsNullOrEmpty(connection) ? new EmpService() : new EmpService(connection);
                    break;
            }

            return obj;

        }

    }
}
