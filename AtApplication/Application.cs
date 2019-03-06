using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AtCommon;
using AtCommon.ApplicationInterfaces;

namespace AtApplication
{
	public abstract class Application
	{
		private readonly IDictionary<Type, IEnumerable<MethodInfo>> _application;

		public Application()
		{
			var controllers = Assembly.GetExecutingAssembly().GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(IController)));

			this._application = controllers.ToDictionary(
				x => x,
				x => x.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(
					m => m.GetParameters().Length == 1 &&
					   m.GetParameters().First().ParameterType == typeof(AtHttpRequest) &&
						 m.ReturnType.IsSubclassOf(typeof(HttpResponse))
					   ).ToList().AsEnumerable());
		}

		public virtual Response Process(AtHttpRequest request)
		{
			if (request.Uri.Segments.Length < 3)
			{
				return new HttpResponse();
			}

			var controllerName = request.Uri.Segments[1];
			var actionName = request.Uri.Segments[2];

			var controller = _application.Keys.FirstOrDefault(x => x.Name.ToLower().Contains(controllerName));

			if (controller == null)
			{
				return new HttpResponse();
			}

			var action = _application[controller].FirstOrDefault(x => x.Name.ToLower().Contains(actionName));

			if (action == null)
			{
				return new HttpResponse();
			}

			return (HttpResponse)(action.Invoke(Activator.CreateInstance(controller), new object[] { request.Parameters }));
		}
	}

	public class Application1 : Application
	{
		public override Response Process(AtHttpRequest req)
		{
			return new HttpResponse();
		}
	}
}
