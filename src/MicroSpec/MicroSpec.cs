using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace MicroSpec
{
    public interface IGivenHelper
    {
        IGivenHelper And(Action step);
        IGivenHelper And<T>(Action<T> step, T arg1);
        IGivenHelper And<T, TU>(Action<T, TU> step, T arg1, TU arg2);
        IGivenHelper And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3);
        IGivenHelper And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4);
        IGivenHelper And<T, TU, TV, TW, TX>(Action<T, TU, TV, TW, TX> step, T arg1, TU arg2, TV arg3, TW arg4, TX arg5);

        IWhenHelper When(Action step);
        IWhenHelper When<T>(Action<T> step, T arg1);
        IWhenHelper When<T, TU>(Action<T, TU> step, T arg1, TU arg2);
        IWhenHelper When<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3);
        IWhenHelper When<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4);
    }

    public interface IWhenHelper
    {
        IWhenHelper And(Action step);
        IWhenHelper And<T>(Action<T> step, T arg1);
        IWhenHelper And<T, TU>(Action<T, TU> step, T arg1, TU arg2);
        IWhenHelper And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3);
        IWhenHelper And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4);

        IThenHelper Then(Action step);
        IThenHelper Then<T>(Action<T> step, T arg1);
        IThenHelper Then<T, TU>(Action<T, TU> step, T arg1, TU arg2);
        IThenHelper Then<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3);

        IExceptionHelper ThenA<T>() where T : Exception;
        IExceptionHelper ThenAn<T>() where T : Exception;
    }

    public interface IThenHelper
    {
        IThenHelper And(Action step);
        IThenHelper And<T>(Action<T> step, T arg1);
        IThenHelper And<T, TU>(Action<T, TU> step, T arg1, TU arg2);
        IThenHelper And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3);
        IThenHelper And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4);

        IExceptionHelper AndA<T>() where T : Exception;
        IExceptionHelper AndAn<T>() where T : Exception;
    }

    public interface IExceptionHelper
    {
        void IsThrown();
        void IsNotThrown();
        void IsThrownWithMessage(string message);
    }

    [TestFixture]
    public abstract class Specification : IGivenHelper, IWhenHelper, IThenHelper, IExceptionHelper
    {
        [SetUp]
        public void BeforeEachSpecification()
        {
            _caughtException = null;
            var environmentOutputConfig = Environment.GetEnvironmentVariable("MicroSpec.Specification.WriteOutput");


            if (!bool.TryParse(environmentOutputConfig, out var writeOutput))
                writeOutput = true;
            
            if(!writeOutput)
            { Console.SetOut(TextWriter.Null);}
        }

        private Type _expectedExceptionType;
        private Exception _caughtException;

        private static class Prefixes
        {
            public static string Given
            {
                get { return "Given"; }
            }

            public static string When
            {
                get { return "When"; }
            }

            public static string Then
            {
                get { return "Then"; }
            }

            public static string And
            {
                get { return "And"; }
            }

            public static string ThenA
            {
                get { return "Then a"; }
            }

            public static string ThenAn
            {
                get { return "Then an"; }
            }

            public static string AndA
            {
                get { return "And a"; }
            }

            public static string AndAn
            {
                get { return "And an"; }
            }
        }

        public IGivenHelper Given(Action step)
        {
            WriteStep(Prefixes.Given, step.Method);
            step();
            return this;
        }

        public IGivenHelper Given<T>(Action<T> step, T arg1)
        {
            WriteStep(Prefixes.Given, step.Method, arg1);
            step(arg1);
            return this;
        }

        public IGivenHelper Given<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            WriteStep(Prefixes.Given, step.Method, arg1, arg2);
            step(arg1, arg2);
            return this;
        }

        public IGivenHelper Given<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            WriteStep(Prefixes.Given, step.Method, arg1, arg2, arg3);
            step(arg1, arg2, arg3);
            return this;
        }

        public IGivenHelper Given<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            WriteStep(Prefixes.Given, step.Method, arg1, arg2, arg3, arg4);
            step(arg1, arg2, arg3, arg4);
            return this;
        }

        public IWhenHelper When(Action step)
        {
            WriteStep(Prefixes.When, step.Method);
            return TryRunStep(step);
        }

        public IWhenHelper When<T>(Action<T> step, T arg1)
        {
            WriteStep(Prefixes.When, step.Method, arg1);
            return TryRunStep(step, arg1);
        }

        public IWhenHelper When<T,U>(Action<T,U> step, T arg1, U arg2)
        {
            WriteStep(Prefixes.When, step.Method, arg1, arg2);
            step(arg1,arg2);
            return this;
        }

        IGivenHelper IGivenHelper.And(Action step)
        {
            WriteStep(Prefixes.And, step.Method);
            step();
            return this;
        }

        IGivenHelper IGivenHelper.And<T>(Action<T> step, T arg1)
        {
            WriteStep(Prefixes.And, step.Method, arg1);
            step(arg1);
            return this;
        }

        IGivenHelper IGivenHelper.And<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2);
            step(arg1, arg2);
            return this;
        }

        IGivenHelper IGivenHelper.And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3);
            step(arg1, arg2, arg3);
            return this;
        }

        IGivenHelper IGivenHelper.And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3, arg4);
            step(arg1, arg2, arg3, arg4);
            return this;
        }

        public IGivenHelper And<T, TU, TV, TW, TX>(Action<T, TU, TV, TW, TX> step, T arg1, TU arg2, TV arg3, TW arg4, TX arg5)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3, arg4, arg5);
            step(arg1, arg2, arg3, arg4, arg5);
            return this;
        }

        IWhenHelper IGivenHelper.When(Action step)
        {
            WriteStep(Prefixes.When, step.Method);
            return TryRunStep(step);
        }

        IWhenHelper IGivenHelper.When<T>(Action<T> step, T arg1)
        {
            WriteStep(Prefixes.When, step.Method, arg1);
            return TryRunStep(step, arg1);
        }

        IWhenHelper IGivenHelper.When<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            WriteStep(Prefixes.When, step.Method, arg1, arg2);
            return TryRunStep(step, arg1, arg2);
        }

        IWhenHelper IGivenHelper.When<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            WriteStep(Prefixes.When, step.Method, arg1, arg2, arg3);
            return TryRunStep(step, arg1, arg2, arg3);
        }

        public IWhenHelper When<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            WriteStep(Prefixes.When, step.Method, arg1, arg2, arg3, arg4);
            return TryRunStep(step, arg1, arg2, arg3, arg4);
        }

        IWhenHelper IWhenHelper.And(Action step)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.And, step.Method);
            return TryRunStep(step);
        }

        IWhenHelper IWhenHelper.And<T>(Action<T> step, T arg1)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.And, step.Method, arg1);
            return TryRunStep(step, arg1);
        }

        IWhenHelper IWhenHelper.And<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.And, step.Method, arg1, arg2);
            return TryRunStep(step, arg1, arg2);
        }

        IWhenHelper IWhenHelper.And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3);
            return TryRunStep(step, arg1, arg2, arg3);
        }

        IWhenHelper IWhenHelper.And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3, arg4);
            return TryRunStep(step, arg1, arg2, arg3, arg4);
        }

        private IWhenHelper TryRunStep(Action step)
        {
            try
            {
                step();
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
            return this;
        }

        private IWhenHelper TryRunStep<T>(Action<T> step, T arg1)
        {
            try
            {
                step(arg1);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
            return this;
        }

        private IWhenHelper TryRunStep<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            try
            {
                step(arg1, arg2);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
            return this;
        }

        private IWhenHelper TryRunStep<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            try
            {
                step(arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
            return this;
        }

        private IWhenHelper TryRunStep<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            try
            {
                step(arg1, arg2, arg3, arg4);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
            return this;
        }

        IThenHelper IThenHelper.And(Action step)
        {
            WriteStep(Prefixes.And, step.Method);
            step();
            return this;
        }

        IThenHelper IThenHelper.And<T>(Action<T> step, T arg1)
        {
            WriteStep(Prefixes.And, step.Method, arg1);
            step(arg1);
            return this;
        }

        IThenHelper IThenHelper.And<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2);
            step(arg1, arg2);
            return this;
        }

        IThenHelper IThenHelper.And<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3);
            step(arg1, arg2, arg3);
            return this;
        }
        public IThenHelper And<T, TU, TV, TW>(Action<T, TU, TV, TW> step, T arg1, TU arg2, TV arg3, TW arg4)
        {
            WriteStep(Prefixes.And, step.Method, arg1, arg2, arg3, arg4);
            step(arg1, arg2, arg3, arg4);
            return this;
        }

        IThenHelper IWhenHelper.Then(Action step)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.Then, step.Method);
            step();
            return this;
        }

        IThenHelper IWhenHelper.Then<T>(Action<T> step, T arg1)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.Then, step.Method, arg1);
            step(arg1);
            return this;
        }

        IThenHelper IWhenHelper.Then<T, TU>(Action<T, TU> step, T arg1, TU arg2)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.Then, step.Method, arg1, arg2);
            step(arg1, arg2);
            return this;
        }

        IThenHelper IWhenHelper.Then<T, TU, TV>(Action<T, TU, TV> step, T arg1, TU arg2, TV arg3)
        {
            ThrowCaughtException();
            WriteStep(Prefixes.Then, step.Method, arg1, arg2, arg3);
            step(arg1, arg2, arg3);
            return this;
        }

        IExceptionHelper IWhenHelper.ThenA<T>()
        {
            WriteStep(Prefixes.ThenA, typeof (T));
            _expectedExceptionType = typeof (T);
            return this;
        }

        IExceptionHelper IWhenHelper.ThenAn<T>()
        {
            WriteStep(Prefixes.ThenAn, typeof (T));
            _expectedExceptionType = typeof (T);
            return this;
        }

        IExceptionHelper IThenHelper.AndA<T>()
        {
            WriteStep(Prefixes.AndA, typeof (T));
            _expectedExceptionType = typeof (T);
            return this;
        }

        IExceptionHelper IThenHelper.AndAn<T>()
        {
            WriteStep(Prefixes.AndAn, typeof (T));
            _expectedExceptionType = typeof (T);
            return this;
        }

        void IExceptionHelper.IsThrown()
        {
            Assert.That(_caughtException, Is.Not.Null);
            Assert.That(_caughtException, Is.TypeOf(_expectedExceptionType));
            Console.Write(" is thrown");
        }

        void IExceptionHelper.IsThrownWithMessage(string message)
        {
            Assert.That(_caughtException, Is.Not.Null);
            Assert.That(_caughtException, Is.TypeOf(_expectedExceptionType));
            Assert.AreEqual(message, _caughtException.Message, "Exception was thrown but message differed to expected value.");
            Console.Write(" is thrown with specific message");
        }

        void IExceptionHelper.IsNotThrown()
        {
            Console.Write(" is not thrown");
            ThrowCaughtException();
        }

        private void ThrowCaughtException()
        {
            if (_caughtException != null)
                throw new ApplicationException("An exception was thrown while running the test...", _caughtException);
        }

        private void WriteStep(string prefix, MethodInfo method, params object[] args)
        {
            Console.Write(FormatStepOutput(prefix, method, args));
        }

        private void WriteStep(string prefix, Type exceptionType)
        {
            Console.Write("{0} {1}", prefix, exceptionType.Name);
        }

        private string FormatStepOutput(string prefix, MethodInfo method, params object[] args)
        {
            var genericArgs = method.GetGenericArguments().Cast<object>().ToList();
            var format = method.Name;
            if (genericArgs.Count > 0)
            {
                for (var i = 0; i < genericArgs.Count; i++)
                    format = format.Replace(string.Concat("_g", i, "_"), String.Concat(" {", i, "} "));

                format = string.Format(format, genericArgs.ToArray());
            }


            
            for (var i = 0; i < args.Length; i++)
                format = format.Replace(String.Concat("_", i, "_"), String.Concat(" {", i, "} "));

            format = format.Replace("_", " ");
            var methodName = string.Format(format, args.Select(a => a ?? "null").ToArray());

            return string.Format("{0}{1} {2}{3}", prefix == Prefixes.And ? "  " : string.Empty, prefix,
                                 methodName, Environment.NewLine);
        }
    }
}
