using System;
using System.Runtime.CompilerServices;

namespace ME.ECS.Serializer.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class OrderAttribute : Attribute
	{
		private readonly int order_;
		public OrderAttribute([CallerLineNumber] int order = 0)
		{
			order_ = order;
		}

		public int Order { get { return order_; } }
	}
}