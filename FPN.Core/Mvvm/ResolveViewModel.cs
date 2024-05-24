using System.Diagnostics;
using System.Windows.Markup;

namespace FPN.Core.Mvvm
{
	public class ResolveViewModel : MarkupExtension
	{
		public static Func<Type, object>? Resolver { get; set; }

		public Type? Type { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			Debug.Assert(Type is not null);
			return Resolver?.Invoke(Type) ?? throw new ArgumentNullException($"Unable to resolve type {Type}");
		}
	}
}