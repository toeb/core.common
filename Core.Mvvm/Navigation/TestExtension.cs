using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Controls;

namespace Core.Common.Wpf.Export
{



    public class TestExtension : EvaluatingExtensionBase
    {
        public TestExtension() : base(new object[] { }) { }
        public TestExtension(object arg1) : base(new[] { arg1 }) { }
        public TestExtension(object arg1, object arg2) : base(new[] { arg1, arg2 }) { }
        public TestExtension(object arg1, object arg2, object arg3) : base(new[] { arg1, arg2, arg3 }) { }
        public TestExtension(object arg1, object arg2, object arg3, object arg4) : base(new[] { arg1, arg2, arg3, arg4 }) { }
        public TestExtension(object arg1, object arg2, object arg3, object arg4, object arg5) : base(new[] { arg1, arg2, arg3, arg4, arg5 }) { }
        public TestExtension(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6) : base(new[] { arg1, arg2, arg3, arg4, arg5, arg6 }) { }
        public TestExtension(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7) : base(new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }) { }

        protected override object Evaluate(EvaluatingExtensionBase root, object[] values)
        {

            return string.Join(", ", values);
        }
    }
}