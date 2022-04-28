using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using NUnit.Framework;
using System;
using System.Drawing;

namespace FlaUIPractice.Helpers
{
    public static class Utils
    {
        public static T WaitForElement<T>(Func<T> lambdaCondition, TimeSpan timeoutSeconds)
        {
            var retry = Retry.WhileNull<T>(lambdaCondition, timeoutSeconds, TimeSpan.FromSeconds(1));

            if (!retry.Success)
            {
                Assert.Fail(message: $"Failed to get element within {timeoutSeconds} seconds.");
            }

            return retry.Result;
        }

        public static Point GetClickingPointOfElement(AutomationElement element)
        {
            var pointX = element.BoundingRectangle.X + 10;
            var pointY = element.BoundingRectangle.Y + 10;

            return new Point(pointX, pointY);
        }

        public static void ClickElement(this AutomationElement element)
        {
            var point = GetClickingPointOfElement(element);
            Mouse.MoveTo(point);
            Mouse.Click(point);
        }
    }
}
