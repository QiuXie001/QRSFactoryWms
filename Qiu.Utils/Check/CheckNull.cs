using Qiu.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiu.Utils.Check
{
    public static class CheckNull
    {
        public static void ArgumentIsNullException<TArgument>(TArgument argument, string argumentName = "不能为空")
            where TArgument : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void ArgumentIsNullException(string argument, string argumentName = "不能为空")
        {
            if (argument.IsNull2())
                throw new ArgumentException(argumentName);
        }

        public static void ArgumentIsNullException(string argumentName = "不能为空")
        {
            throw new ArgumentException(argumentName);
        }

        public static void ThrowException(string argumentName = "未实现")
        {
            throw new Exception(argumentName);
        }
    }
}
