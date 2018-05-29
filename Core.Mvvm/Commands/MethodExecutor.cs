using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Core.Common.Injection;

namespace Core.Common.MVVM
{

	public class MethodExecutor
	{
		InjectionService injector = null;
		MethodInfo method;
		IDictionary<int, object> parameters;

		public bool IsValid
		{
			get
			{
				return injector.GetParametersSet(method, parameters).Count == method.GetParameters().Length;
			}
		}

		public MethodExecutor(MethodInfo method)
		{
			this.method = method;
			parameters = new Dictionary<int, object>();
		}

		public void SetParameter(int i, object value)
		{
			parameters[i] = value;
		}
		public void UnsetParameter(int i)
		{
			parameters.Remove(i);
		}

		public void AddService(object service)
		{
			injector.RegisterService(service);
		}
		public bool TryExecute(object instance, out object result)
		{
			var parameterSet = injector.GetParametersSet(method, parameters);
			if (parameterSet.Count != method.GetParameters().Length)
			{
				result = null;
				return false;
			}
			result = injector.Execute(method, instance, parameterSet);
			return true;
		}


	}
}