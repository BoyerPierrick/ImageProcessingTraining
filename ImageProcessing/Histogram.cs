namespace TestHisto
{
    using OpenCvSharp;

    public static class Histogram
    {
        /// <summary>
        /// Retrieves an image as a grayscale OpenCV Mat object.
        /// Change the Path to load your image.
        /// </summary>
        /// <returns>An OpenCV Mat representing the grayscale image.</returns>
        /// <exception cref="Exception">Thrown when the file path is null or invalid.</exception>
        public static Mat GetImage(ImreadModes mode)
        {
            string file = @""
               ?? throw new Exception();

            return new Mat(file, mode);
        }

        /// <summary>
        /// Calculates the distance between two vectors represented as float arrays.
        /// </summary>
        /// <param name="vect1">The first vector.</param>
        /// <param name="vect2">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float GetDistance(float[] vect1, float[] vect2)
        {
            float res = 0;

            int a = vect1.GetLength(0);
            int b = vect2.GetLength(0);

            if (a == b)
            {
                Parallel.For(0, vect1.Length, i =>
                    res += Math.Abs(vect1[i] - vect2[i])
                );
            }

            Console.WriteLine($"Distance between the 2 RoI vector is: {res}");

            return res;
        }

        /// <summary>
        /// Calculates and returns a vector representing the histogram of pixel intensities in the input image.
        /// </summary>
        /// <param name="src">The input image as an OpenCV Mat.</param>
        /// <param name="beta">The normalization parameter.</param>
        /// <returns>An array representing the histogram vector of the input image.</returns>
        public static float[] GetVector(Mat src, int beta)
        {
            using var hist = new Mat();

            Cv2.CalcHist(
                images: [src],
                channels: [0],
                mask: null,
                hist: hist,
                dims: 1,
                histSize: [256],
                ranges: new[] { new Rangef(0, 256) });

            hist.Normalize(alpha: 0, beta: beta, normType: NormTypes.MinMax, dtype: -1, mask: null).GetArray(out float[] res);

            return res;
        }

        /// <summary>
        /// Draws and returns a histogram image of pixel intensities in the input image.
        /// </summary>
        /// <param name="src">The input image as an OpenCV Mat.</param>
        /// <returns>An OpenCV Mat representing the histogram image.</returns>
        public static Mat DrawHistogram(Mat src)
        {
            const int histW = 500;
            const int histH = 400;

            var bin = Math.Round((double)histH / 256);

            using var hist = new Mat();
            
            Cv2.CalcHist(
                images: [src],
                channels: [0],
                mask: null,
                hist: hist,
                dims: 1,
                histSize: [256],
                ranges: new[] { new Rangef(0, 256) });
            
            Mat histImg = new (histH, histW, MatType.CV_8UC3, Scalar.All(255));
            Cv2.Normalize(src: hist, dst: hist, alpha: 0, beta: 256, normType: NormTypes.MinMax, dtype: -1, mask: null);
            
            Parallel.For(0, 256,
                i =>
                {
                    var pt1 = new Point2d(bin * (i - 1), histH - Math.Round(hist.At<float>(i - 1)));
                    var pt2 = new Point2d(bin * (i), histH - Math.Round(hist.At<float>(i)));
                    Cv2.Line(
                        img: histImg,
                        pt1: (Point)pt1,
                        pt2: (Point)pt2,
                        Scalar.Red,
                        1,
                        LineTypes.Link8);
                }
            );

            return histImg;
        }
    }
}
