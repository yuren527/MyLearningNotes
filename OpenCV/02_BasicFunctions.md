## Read from image, video and webcam
```C++
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <iostream>

using namespace std;
using namespace cv;

void ReadImage(string path) {
	Mat img = imread(path);
	imshow("Image", img);
	waitKey(0);
}

void ReadVideo(string path) {
	VideoCapture cap(path);
	Mat img;

	while (true) {	
		cap.read(img);
		imshow("Video", img);
    // When finish it comes a exception,  because there's no more image to display at the end of the video;
		waitKey(20); // Wait for 20 milisecond;
	}
}

void ReadWebcam(int camId) {
	VideoCapture cap(camId);
	Mat img;

	while (true) {	
		cap.read(img);
		imshow("Video", img);
    // When finish it comes a exception,  because there's no more image to display at the end of the video;
		waitKey(1);
	}
}

int main() {
	//ReadImage("Resources/opencvtest.jpeg");
	//ReadVideo("Resources/opencvtest.mp4");
	ReadWebcam(0);
	return 0;
}
```
Reading from webcam is almost the same with reading from video, just need to find out the camera id

# Basic functions
- Convert color
- Gaussian Blur
- Canny, find the edges of the image
- Dilation
- Erosion
- Resize, with a specific size or ratio
- Crop, use functor of the Mat
```C++
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <iostream>

using namespace std;
using namespace cv;

int main() {
	Mat img = imread("Resources/opencvtest.jpeg");
	Mat imgGray, imgBlur, imgCanny, imgDilation, imgErode;
	Mat imgResize, imgResize_Ratio, imgCrop;

	cvtColor(img, imgGray, COLOR_BGR2GRAY);
	GaussianBlur(img, imgBlur, Size(7, 7), 7);
	Canny(img, imgCanny, 50, 150);

	Mat kernel = getStructuringElement(MORPH_RECT, Size(3,3));
	dilate(imgCanny, imgDilation, kernel);
	erode(imgDilation, imgErode, kernel);

	cout << img.size() << endl;
	resize(img, imgResize, Size(500, 500));
	resize(img, imgResize_Ratio, Size(), 0.5, 0.5);
	Rect roi(100, 100, 400, 400);
	imgCrop = img(roi);

	imshow("Image", img);
	imshow("Image Gray", imgGray);
	imshow("Image Blur", imgBlur);
	imshow("Image Canny", imgCanny);
	imshow("Image Dilation", imgDilation);
	imshow("Image Erode", imgErode);
	imshow("Image Resize", imgResize);
	imshow("Image Resize Ratio", imgResize_Ratio);
	imshow("Image Crop", imgCrop);

	waitKey(0);

  return 0;
}
```
# Drawing shapes and text
```C++
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/imgproc.hpp>
#include <iostream>

using namespace std;
using namespace cv;

int main() {
	Mat img(512, 512, CV_8UC3, Scalar(255, 255, 255));

	circle(img, Point(256, 256), 155, Scalar(0, 69, 255), FILLED);
	rectangle(img, Point(130, 226), Point(382, 286), Scalar(255, 255, 255), FILLED);
	rectangle(img, Rect(226, 130, 60, 252), Scalar(255, 255, 255), FILLED);
	line(img, Point(130, 296), Point(382, 296), Scalar(255, 255, 255));
	putText(img, "Markus Gao", Point(137, 262), 1.5, FONT_HERSHEY_DUPLEX, Scalar(0, 69, 255), 3);
	imshow("Image", img);

	waitKey(0);
	return 0;
}
```
- `CV_8UC3` means 8 bits unsigned 3-chanel color(RGB)
- Use Scalar() to specify color
- rectangle can be draw with `rect` or `point`
