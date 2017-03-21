using System;
using System.Collections;
using System.Collections.Generic;
using static System.Console;

namespace CSharp7Features
{
    public static class Tuples
    {
        class ActionResult
        {
            public bool IsFailed { get; set; }
            public string Message { get; set; }
        }

        static ActionResult ProcessOld()
        {
            return new ActionResult()
            {
                IsFailed = true,
                Message = "Not Found"
            };
        }

        static (bool IsFailed, string Message) ProcesNew()
        {
            return (true, "Not Found");
        }

        public static void Foo()
        {
            // Old Way
            var actionResult = ProcessOld();
            if (actionResult.IsFailed) WriteLine(actionResult.Message);

            // New Way
            var newResult = ProcesNew();
            if (newResult.Item1) WriteLine(newResult.Item2); // tuple standard
            if (newResult.IsFailed) WriteLine(newResult.Message); // named properties

            // Explicit result definition
            (bool, string) explicitResult = ProcesNew();
            if (explicitResult.Item1) WriteLine(explicitResult.Item2);

            // Explicit rename result definition
            (bool HasFailed, string Reason) renamed = ProcesNew();
            if (renamed.HasFailed) WriteLine(renamed.Reason);

            // Deconstruction explicit
            (bool hasFailed, string reason) = ProcesNew();
            if (hasFailed) WriteLine(reason);

            // Deconstruction inference
            var (HasFailed, Reason) = ProcesNew();
            if (HasFailed) WriteLine(Reason);
        }
    }
}