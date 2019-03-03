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

		protected Application()
		{
			var controllers = Assembly.GetExecutingAssembly().GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(IController)));

			this._application = controllers.ToDictionary(
				x => x,
				x => x.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(
					m => m.GetParameters().Length == 1 &&
					   m.GetParameters().First().ParameterType == typeof(AtHttpRequest)
					   ).ToList().AsEnumerable());
		}

		public virtual string Process(AtHttpRequest request)
		{
			if (request.Uri.Segments.Length < 3)
			{
				return string.Empty;
			}

			var controllerName = request.Uri.Segments[1];
			var actionName = request.Uri.Segments[2];

			var controller = _application.Keys.FirstOrDefault(x => x.Name.ToLower().Contains(controllerName));

			if (controller == null)
			{
				return string.Empty;
			}

			var action = _application[controller].FirstOrDefault(x => x.Name.ToLower().Contains(actionName));

			if (action == null)
			{
				return string.Empty;
			}

			return action.Invoke(Activator.CreateInstance(controller), new object[] { request.Parameters }).ToString();
		}
	}

	public class Application1 : Application
	{
		public override string Process(AtHttpRequest req)
		{
			return @"{
						""userId"": 1,
						""id"": 1,
						""title"": ""delectus aut autem"",
						""completed"": false
					}";
		}
	}
}
