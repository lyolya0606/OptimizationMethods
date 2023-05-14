using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChartDirector;

namespace OptimizationMethods {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Calculation _calculation;

        public MainWindow() {
            InitializeComponent();

            _calculation = new(double.Parse(learningRateTextBox.Text), double.Parse(momentumTextBox.Text));
            FillResult();
            CreateChart2D(WPFChart2D);
            CreateChart3D(WPFChart3D);

        }

        private void FillResult() {
            List<Pair> listOfPairsGr = _calculation.GradientDescent();
            Pair lastPairGr = new(listOfPairsGr[listOfPairsGr.Count - 1].X, listOfPairsGr[listOfPairsGr.Count - 1].Y);
            LResult.Content = Math.Round(lastPairGr.X, 4);
            SResult.Content = Math.Round(lastPairGr.Y, 4);
            PResult.Content = Math.Round(_calculation.Function(lastPairGr) * 100, 4);
            
            List<Pair> listOfPairs = _calculation.GradientDescentHeavyBall();
            Pair lastPair = new(listOfPairs[listOfPairs.Count - 1].X, listOfPairs[listOfPairs.Count - 1].Y);
            LResultBall.Content = Math.Round(lastPair.X, 4);
            SResultBall.Content = Math.Round(lastPair.Y, 4);
            PResultBall.Content = Math.Round(_calculation.Function(lastPair) * 100, 4);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool CheckTextBox() {
            return double.Parse(learningRateTextBox.Text) >= 0 && double.Parse(learningRateTextBox.Text) <= 1 &&
                   double.Parse(momentumTextBox.Text) <= 1 && double.Parse(momentumTextBox.Text) >= 0;
        }
        
        private bool CheckTextBoxEmpty() {
            return learningRateTextBox.Text != "" && momentumTextBox.Text != "";
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e) {
            if (!CheckTextBox()) {
                MessageBox.Show("Коэффициенты должны быть в диапазоне от 0 до 1!");
            } else if (!CheckTextBoxEmpty()) {
                MessageBox.Show("Введите данные");
            } else {
                _calculation = new(double.Parse(learningRateTextBox.Text), double.Parse(momentumTextBox.Text));
                FillResult();
                CreateChart2D(WPFChart2D);
                CreateChart3D(WPFChart3D);
            }
        }


        private void CalcButton_MouseDown(object sender, MouseButtonEventArgs e) {
            //if (!CheckTextBox()) {
            //    MessageBox.Show("Коэффициенты должны быть в диапазоне от 0 до 1!");
            //} else if (!CheckTextBoxEmpty()) {
            //    MessageBox.Show("Введите данные");
            //} else {
            //    _calculation = new(double.Parse(learningRateTextBox.Text), double.Parse(momentumTextBox.Text));
            //    FillResult();
            //    CreateChart2D(WPFChart2D);
            //    CreateChart3D(WPFChart3D);
            //}
        }


        //Name of demo module
        // public string getName() { return "Contour Chart"; }
        //
        //
        // public int getNoOfCharts() { return 1; }


        private void CreateChart2D(WPFChartViewer viewer) {
            double[] dataX = _calculation.GetL().ToArray();
            double[] dataY = _calculation.GetS().ToArray();
            double[] dataZ = new double[dataX.Length * dataY.Length];
            for (int yIndex = 0; yIndex < dataY.Length; ++yIndex) {
                double y = dataY[yIndex];
                for (int xIndex = 0; xIndex < dataX.Length; ++xIndex) {
                    double x = dataX[xIndex];
                    Pair pair = new(x, y);
                    //dataZ[yIndex * dataX.Length + xIndex] = t < 12 ? t : double.NaN;
                    dataZ[yIndex * dataX.Length + xIndex] = _calculation.Function(pair);
                }
            }
            
            List<Pair> pointsGradient = _calculation.GradientDescent();
            double[] lineXGradient = new double[pointsGradient.Count];
            double[] lineYGradient = new double[pointsGradient.Count];

            for (int i = 0; i < pointsGradient.Count; i++) {
                lineXGradient[i] = pointsGradient[i].X;
                lineYGradient[i] = pointsGradient[i].Y;
            }
            
            List<Pair> pointsHeavyBall = _calculation.GradientDescentHeavyBall();
            double[] lineXHeavyBall = new double[pointsHeavyBall.Count];
            double[] lineYHeavyBall = new double[pointsHeavyBall.Count];

            for (int i = 0; i < pointsHeavyBall.Count; i++) {
                lineXHeavyBall[i] = pointsHeavyBall[i].X;
                lineYHeavyBall[i] = pointsHeavyBall[i].Y;
            }
            
            List<Pair> grad = _calculation.GradientDescent();
            // var joinedNames = x.Aggregate((a, b) => a.ToString() + b.ToString());
            Pair lastPair = new(grad[grad.Count - 1].X, grad[grad.Count - 1].Y);
            double[] lastX = new[] { grad[grad.Count - 1].X };
            double[] lastY = new[] { grad[grad.Count - 1].Y };
            
            XYChart c = new XYChart(650, 650);
            //c.addTitle("z = x * sin(y) + y * sin(x)      ", "Arial Bold Italic", 15);
            c.setPlotArea(75, 20, 500, 550, -1, -1, -1, c.dashLineColor(unchecked((int)0x80000000),
                Chart.DotLine), -1);
            // c.xAxis().setTitle("X-Axis Title Place Holder", "Arial Bold Italic", 12);
            // c.yAxis().setTitle("Y-Axis Title Place Holder", "Arial Bold Italic", 12);
            c.xAxis().setLabelStyle("Arial Bold");
            c.yAxis().setLabelStyle("Arial Bold");
       
            c.yAxis().setTickDensity(30);
            c.xAxis().setTickDensity(30);
            
            var dot = c.addLineLayer(lastY, 0xff0000);
            dot.setXData(lastX);

            dot.setLineWidth(5);
            
            var lGradient = c.addLineLayer(lineYGradient, c.dashLineColor(0x808080, Chart.DashLine));
            lGradient.setXData(lineXGradient);
            lGradient.setLineWidth(4);
            
            var lHeavyBall = c.addLineLayer(lineYHeavyBall, 0x000000);
            lHeavyBall.setXData(lineXHeavyBall);
            lHeavyBall.setLineWidth(5);
            


            ContourLayer layer = c.addContourLayer(dataX, dataY, dataZ);

            c.getPlotArea().moveGridBefore(layer);
            c.getPlotArea().moveGridBefore(lGradient);
            c.getPlotArea().moveGridBefore(lHeavyBall);
            c.getPlotArea().moveGridBefore(dot);

            ColorAxis cAxis = layer.setColorAxis(580, 60, Chart.TopLeft, 505, Chart.Right);
            
            cAxis.setTitle("Color Legend", "Arial Bold Italic", 12);
            cAxis.setLabelStyle("Arial Bold");
            viewer.Chart = c;
            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='<*cdml*><*font=bold*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}''");
        }
        
        
         private void CreateChart3D(WPFChartViewer viewer) {
            double[] dataX = _calculation.GetL().ToArray();
            double[] dataY = _calculation.GetS().ToArray();
            double[] dataZ = new double[dataX.Length * dataY.Length];
            for (int yIndex = 0; yIndex < dataY.Length; ++yIndex) {
                double y = dataY[yIndex];
                for (int xIndex = 0; xIndex < dataX.Length; ++xIndex) {
                    double x = dataX[xIndex];
                    Pair pair = new(x, y);
                    //double t = Math.Pow((x - y), 2) + Math.Pow((x + y - 10), 2) / 9;
                    dataZ[yIndex * dataX.Length + xIndex] = _calculation.Function(pair);
                    //dataZ[yIndex * dataX.Length + xIndex] = t;
                }
            }

            List<Pair> grad = _calculation.GradientDescent();
            // var joinedNames = x.Aggregate((a, b) => a.ToString() + b.ToString());
            Pair lastPair = new(grad[grad.Count - 1].X, grad[grad.Count - 1].Y);
            double[] lastX = new[] { grad[grad.Count - 1].X };
            double[] lastY = new[] { grad[grad.Count - 1].Y };
            double[] lastZ = new[] { dataZ[dataZ.Length - 1] };
            
            
            // Create a SurfaceChart object of size 720 x 600 pixels
            SurfaceChart c = new SurfaceChart(600, 600);

            //c.setData(lastX, lastY, lastZ);
            //c.addSurfaceLine(lastX, lastY, 0x000000);
            //c.setSize(600, 600);

            // Set the center of the plot region at (330, 290), and set width x depth x height to
            // 360 x 360 x 270 pixels
            c.setPlotRegion(330, 290, 380, 380, 300);
            

            // Set the data to use to plot the chart
            c.setData(dataX, dataY, dataZ);

            // Spline interpolate data to a 80 x 80 grid for a smooth surface
            c.setInterpolation(80, 80);

            // Set the view angles
            c.setViewAngle(m_elevationAngle, m_rotationAngle);
            
            // Check if draw frame only during rotation
            if (m_isDragging)
                c.setShadingMode(Chart.RectangularFrame);

            // Add a color axis (the legend) in which the left center is anchored at (660, 270). Set
            // the length to 200 pixels and the labels on the right side.
            c.setColorAxis(650, 270, Chart.Left, 200, Chart.Right);

            // Set the x, y and z axis titles using 10 points Arial Bold font
            c.xAxis().setTitle("X", "Arial Bold", 15);
            c.yAxis().setTitle("Y", "Arial Bold", 15);

            // Set axis label font
            c.xAxis().setLabelStyle("Arial Bold", 10);
            c.yAxis().setLabelStyle("Arial Bold", 10);
            c.zAxis().setLabelStyle("Arial Bold", 10);
            c.colorAxis().setLabelStyle("Arial Bold", 10);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "",
                "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
        }
         
         // 3D view angles
         private double m_elevationAngle = 30;
         private double m_rotationAngle = 45;

         // Keep track of mouse drag
         private int m_lastMouseX = -1;
         private int m_lastMouseY = -1;
         private bool m_isDragging = false;
         
         
         
         private void WPFChart3D_MouseMoveChart(object sender, MouseEventArgs e) {
             WPFChart3D.updateViewPort(true, false);
             int mouseX = WPFChart3D.ChartMouseX;
             int mouseY = WPFChart3D.ChartMouseY;

             // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
             if (Mouse.LeftButton == MouseButtonState.Pressed) {
                 if (m_isDragging) {
                     m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                     m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                     WPFChart3D.updateViewPort(true, false);
                 }
                 
                 m_lastMouseX = mouseX;
                 m_lastMouseY = mouseY;
                 m_isDragging = true;
             }
         }
         
         private void WPFChart3D_ViewPortChanged(object sender, WPFViewPortEventArgs e) {
             if (e.NeedUpdateChart)
                 CreateChart3D((WPFChartViewer)sender);
         }
         private void WPFChart3D_MouseUpChart(object sender, MouseEventArgs e) {
             m_isDragging = false;
             WPFChart3D.updateViewPort(true, false);
         }

        private void Info_Click(object sender, RoutedEventArgs e) {
            InfoWindow info = new();
            info.ShowDialog();
        }

    }
}
