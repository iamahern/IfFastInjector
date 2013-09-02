using NUnit.Framework;
using System;

namespace IfInjectorTest.Factory
{
	[TestFixture()]
	public class ImplicitTypeTest : IfInjectorTest.Basic.ImplicitTypeTest
	{
		public ImplicitTypeTest() {
			IsFactory = true;
		}
	}
}

