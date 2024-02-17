using OpenCvSharp;
using TestHisto;

Mat src = Histogram.GetImage(ImreadModes.Grayscale);

// Define the Region of Interest. You can change the
// size of the RoI.
Rect roi1 = new (50, 50, 70, 70);
Rect roi2 = new (250, 250, 70, 70);

// Create matrix of the RoI.
Mat mat1 = new(src, roi1);
Mat mat2 = new(src, roi2);

// Draw roi on the image.
src.Rectangle(rect: roi1, Scalar.Green, thickness: 1);
src.Rectangle(rect: roi2, Scalar.Green, thickness: 1);

// Get the vector value of RoI. Change beta value if
// you want to normalize the data inside the matrix.
int beta = 256;
float[] vecMat1 = Histogram.GetVector(src: mat1, beta);
float[] vecMat2 = Histogram.GetVector(src: mat2, beta);

// Calculate the distance between the 2 RoI.
Histogram.GetDistance(vect1: vecMat1, vect2: vecMat2);

// Open Image.
Cv2.ImShow("Image", src);

// Create the histogram for each RoI. Open windows
// with the graphic.
Mat hist1 = Histogram.DrawHistogram(src: mat1);
Cv2.ImShow("Histogram RoI 1", hist1);

Mat hist2 = Histogram.DrawHistogram(src: mat2);
Cv2.ImShow("Histogram RoI 2", hist2);

Cv2.WaitKey(0);