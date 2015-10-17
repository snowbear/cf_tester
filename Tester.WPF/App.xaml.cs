using System;
using System.Linq;
using System.Windows;

namespace Tester
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			RegisterViews();
			base.OnStartup(e);
		}

		private void RegisterViews()
		{
			var assembly = typeof (App).Assembly;
			foreach (var viewType in assembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.Contains("Views")))
			{
				var viewModelTypeName = viewType.FullName.Replace("View", "ViewModel");
				var viewModelType = assembly.GetType(viewModelTypeName);
				RegisterDataTemplate(viewType, viewModelType);
			}
		}

		private void RegisterDataTemplate(Type viewType, Type viewModelType)
		{
			var dataTemplateKey = new DataTemplateKey(viewModelType);
			var dataTemplate = new DataTemplate(viewModelType)
				{
					VisualTree = new FrameworkElementFactory(viewType),
				};
			Resources.Add(dataTemplateKey, dataTemplate);
		}
	}
}
