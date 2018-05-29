using System;
namespace Core.Common.MVVM
{
    public interface IMethodArguments
    {
        object[] Evaluate(Type[] expectedTypes);
    }

    public interface IArgumentScope
    {
        object[] Capture(object arguments);
    }

}
