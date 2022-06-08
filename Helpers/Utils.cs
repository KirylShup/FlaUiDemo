using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
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

            Utils.SaveToPng(expectedPath, expected);
            Console.WriteLine(@"Expected bitmap saved to" + expectedPath);

            Utils.SaveToPng(actualPath, actual);
            Console.WriteLine(@"Actual bitmap saved to" + actualPath);

            var byteExpected = expected.ImageToByteArray();
            var byteActual = actual.ImageToByteArray();

            if(byteExpected.Length == byteActual.Length)
            {
                var byteDiff = new byte[byteExpected.Length];
                for (int i = 0; i < byteExpected.Length; i++)
                {
                    if((i + 1) % 4 == 0)
                    {
                        byteDiff[i] = (byte)((byteActual[i] + byteExpected[i]) / 2);
                    }
                    else 
                    {
                        byteDiff[i] = (byte)(Math.Abs(byteActual[i] - byteExpected[i]));
                    }
                }

                var diffBmp = new WriteableBitmap(expected.PixelWidth, expected.PixelHeight, expected.DpiX, expected.DpiY, expected.Format, expected.Palette);
                diffBmp.WritePixels(new System.Windows.Int32Rect(0, 0, expected.PixelWidth, expected.PixelHeight), byteDiff, expected.BackBufferStride, 0);
                string diffPath = Path.Combine(path, Path.GetFileNameWithoutExtension(resourceName) + "-diff.png");
                Utils.SaveToPng(diffPath, diffBmp);
                Console.WriteLine(@"Difference bitmap saved to" + diffPath);
            }
        }

        public static void SaveToPng (string fileName, WriteableBitmap bmp)
        {
            var path = Path.GetDirectoryName(fileName);
            if(!string.IsNullOrEmpty(path))
            {
                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));
                using var fileStream = File.Create(fileName);
                pngEncoder.Save(fileStream);
            }
        }

        public static WriteableBitmap LoadFromPng(string fileName)
        {
            WriteableBitmap bmp;

            using (var fileStream = File.OpenRead(fileName))
            {
                bmp = DecodePngStream(fileStream);
            }

            return bmp;
        }

        public static WriteableBitmap ConvertToWritableBitmap(CaptureImage image)
        {
            using var ms = new MemoryStream();
            image.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            return new WriteableBitmap(bitmapSource);
        }

        private static WriteableBitmap DecodePngStream(Stream pngStream)
        {
            var decoder = new PngBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            return new WriteableBitmap(bitmapSource);
        }

        private static byte[] ImageToByteArray(this WriteableBitmap bmp)
        {
            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;
            var stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

            var bitmapData = new byte[height * stride];
            bmp.CopyPixels(bitmapData, stride, 0);

            return bitmapData;
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
