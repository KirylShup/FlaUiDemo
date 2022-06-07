using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

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

        public static bool CompareScreenshots(string resourceName, WriteableBitmap actual, WriteableBitmap expected, double allowedErrorPercentage)
        {
            try
            {
                double averageErrorPercentage = 0.0;
                double maxError = double.MinValue;

                Assert.That(new Size(actual.PixelWidth, actual.PixelHeight), Is.EqualTo(new Size(expected.PixelWidth, expected.PixelHeight)), "Images sizes are different.");

                var px1 = actual.ImageToByteArray();
                var px2 = expected.ImageToByteArray();

                Assert.That(px1.Length, Is.EqualTo(px2.Length), "Images pixel sizes are different.");

                var areEqual = true;

                for(int i = 0; i < px1.Length; i += 4)
                {
                    var b1 = px1[i];
                    var g1 = px1[i + 1];
                    var r1 = px1[i + 2];
                    var a1 = px1[i + 3];

                    var b2 = px2[i];
                    var g2 = px2[i + 1];
                    var r2 = px2[i + 2];
                    var a2 = px2[i + 3];

                    var error = Math.Sqrt((a1 - a2) * (a1 - a2) + (r1 - r2) * (r1 - r2) + (g1 - g2) * (g1 - g2) + (b1 - b2) * (b1 - b2));
                    averageErrorPercentage += error;
                    maxError = Math.Max(averageErrorPercentage, error);
                }

                averageErrorPercentage /= (actual.PixelHeight * actual.PixelWidth * 5.10);
                maxError /= 5.10;
                if (averageErrorPercentage > allowedErrorPercentage)
                {
                    areEqual = false;
                }

                string message = $"Resource: {resourceName}, ImageAveError: {averageErrorPercentage}%, Allowable AveError: {allowedErrorPercentage}%, MaxError: {maxError}%.";
                Assert.That(areEqual, message);
                Console.WriteLine(message);

                return true;
            }

            catch
            {
                Utils.SaveDiffImages(resourceName, actual, expected);

                throw;
            }
        }

        public static void SaveDiffImages(string resourceName, WriteableBitmap actual, WriteableBitmap expected)
        {
            var path = Path.GetDirectoryName(resourceName);
            string expectedPath = Path.Combine(path, Path.GetFileNameWithoutExtension(resourceName) + "-expected.png");
            string actualPath = Path.Combine(path, Path.GetFileNameWithoutExtension(resourceName)+ "-actual.png");

            // saving file logic here


        }

        private static byte[] ImageToByteArray(this WriteableBitmap wbm)
        {
            int stride = wbm.PixelWidth * wbm.Format.BitsPerPixel / 8;
            int size = stride * wbm.PixelHeight;

            byte[] buffer = new byte[size];

            wbm.CopyPixels(buffer, stride, 0);

            return buffer;
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
