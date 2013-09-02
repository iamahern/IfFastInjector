﻿using System;
using NUnit.Framework;
using IfInjector;
using System.Diagnostics;

namespace IfInjectorTest
{
    [TestFixture]
    public class MultipleInjectorsTest
    {
		private Injector injector1 = new Injector();
		private Injector injector2 = new Injector();

        [Test]
        public void RegisterMethodToResolveInterfaceTest()
        {
			MyClass i1expect = new MyClass(), 
					i2expect = new MyClass();

			injector1.Bind<MyClass>().SetFactory(() => i1expect);
			injector2.Bind<MyClass>().SetFactory(() => i2expect);

			var res1 = injector2.Resolve<MyClass>();
			var res2 = injector2.Resolve<MyClass>();
        }

        class MyClass
        {
        }
    }
}
