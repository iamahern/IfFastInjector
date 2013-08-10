using System;
using NUnit.Framework;
using IfInjector;
using IfInjector.IfInjectorTypes;

namespace IfInjectorTest
{
    [TestFixture]
    public class UnitTest2 : Base2WayTest
    {
        class TestClassSetResolverByConstructorInfoTest
        {
            public TestClassSetResolverByConstructorInfoTest() { }
            public TestClassSetResolverByConstructorInfoTest(OtherClassSetResolverByConstructorInfoTest something) { }

            public class OtherClassSetResolverByConstructorInfoTest { }
        }


        [Test]
        public void ResolveByTypeObjectTest()
        {
            // resolve at least twice to execute both code paths
			var myObject = Injector.Resolve(typeof(TestClassTestResolveByTypeObject));
			myObject = Injector.Resolve(typeof(TestClassTestResolveByTypeObject));
			myObject = Injector.Resolve(typeof(TestClassTestResolveByTypeObject));
        }

        class TestClassTestResolveByTypeObject { }

        [Test]
        public void AddPropertyInjectorTest()
        {
			var binding = Bind<TestClassAddPropertyInjectorTest> ();
			binding.AddPropertyInjector((TestClassAddPropertyInjectorTest v) => v.Other);
			binding.AddPropertyInjector((TestClassAddPropertyInjectorTest v) => v.MyStringProperty, () => "Goldfinger");

			var result = Injector.Resolve<TestClassAddPropertyInjectorTest>();

            Assert.AreEqual("Goldfinger", result.MyStringProperty);
            Assert.AreEqual(7, result.Other.Id);
        }

        class TestClassAddPropertyInjectorTest
        {
            public string MyStringProperty { get; set; }
            public OtherClassAddPropertyInjectorTest Other { get; set; }

            public class OtherClassAddPropertyInjectorTest { public int Id { get { return 7; } } }
        }

        [Test]
        public void InterfaceExceptionTest()
        {
			InjectorException exception = null;
            try
            {
				Injector.Resolve<IInterfaceExceptionTest>();
            }
			catch (InjectorException ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        interface IInterfaceExceptionTest { }

        [Test]
        public void AddPropertySetterNotMemberExpression()
        {
			var binding = Bind<TestClassAddPropertyInjectorTest>();

			InjectorException exception = null;
            try
            {
				binding.AddPropertyInjector((TestClassAddPropertyInjectorTest v) => "");
            }
			catch (InjectorException ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
        }

        [Test]
        public void ExceptionConstructorTest()
        {
			var ex1 = new InjectorException(InjectorErrors.ErrorAmbiguousBinding, "foobar");
			var ex2 = new InjectorException(InjectorErrors.ErrorAmbiguousBinding, "something", ex1);

            Assert.IsNotNull(ex1);
            Assert.IsNotNull(ex2);
        }

		[Test]
		public void ExceptionErrorCodeFormattingTest()
		{
			var random = Guid.NewGuid ().ToString ();
			var ex = InjectorErrors.ErrorAmbiguousBinding.FormatEx (random);

			Assert.AreEqual (string.Format (InjectorErrors.ErrorAmbiguousBinding.MessageTemplate, random), ex.Message);
			Assert.AreEqual ("IF0004", InjectorErrors.ErrorAmbiguousBinding.MessageCode);
		}

		class NoProperConstructor {
			private NoProperConstructor() {}
		}

		[Test]
		public void NoProperConstructorTest() {
			var ex1 = InjectorErrors.ErrorNoAppropriateConstructor.FormatEx (typeof(NoProperConstructor).FullName);

			try { 
				Injector.Resolve<NoProperConstructor>();
				Assert.Fail();
			} catch (InjectorException ex) {
				Assert.AreEqual (ex1.ErrorType.MessageCode, ex.ErrorType.MessageCode);
			}
		}

		[Test]
		public void TestVerify() {
			Injector.Bind<NoProperConstructor> ();

			try { 
				Injector.Verify();
				Assert.Fail();
			} catch (InjectorException ex) {
				var ex1 = InjectorErrors.ErrorNoAppropriateConstructor.FormatEx (typeof(NoProperConstructor).FullName);
				Assert.AreEqual (ex1.ErrorType.MessageCode, ex.ErrorType.MessageCode);
			}
		}
    }
}
