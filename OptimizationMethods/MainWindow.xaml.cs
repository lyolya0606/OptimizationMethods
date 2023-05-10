using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow() {
            InitializeComponent();

            Calculation calculation = new();
            // TestTextBox.Text = calculation.Gradient(5, 6)[0].ToString() + " " + calculation.Gradient(5, 6)[1].ToString();

            List<Pair> x = calculation.GradientDescent();
            // var joinedNames = x.Aggregate((a, b) => a.ToString() + b.ToString());
            Pair pair = new(x[x.Count - 1].X, x[x.Count - 1].Y);
            TestTextBox.Text = x[x.Count - 1].X.ToString() + " " + x[x.Count - 1].Y.ToString() + "\n" + calculation.Function(pair).ToString();
            string s = "";
            for (int i = 0; i < x.Count; i++) {
                s += x[i].ToString() + "\n";
            }
            //TestTextBox.Text = calculation.Result().ToString();

            CreateChart2D(WPFChart2D);
            CreateChart3D(WPFChart3D);

        }

        private Calculation _calculation = new();


        //Name of demo module
        // public string getName() { return "Contour Chart"; }
        //
        //
        // public int getNoOfCharts() { return 1; }


        public void CreateChart2D(WPFChartViewer viewer) {
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
            
            List<Pair> points = _calculation.GradientDescent();
            double[] lineX = new double[points.Count];
            double[] lineY = new double[points.Count];

            for (int i = 0; i < points.Count; i++) {
                lineX[i] = points[i].X;
                lineY[i] = points[i].Y;
            }
            XYChart c = new XYChart(600, 600);
            //c.addTitle("z = x * sin(y) + y * sin(x)      ", "Arial Bold Italic", 15);
            c.setPlotArea(75, 20, 500, 550, -1, -1, -1, c.dashLineColor(unchecked((int)0x80000000),
                Chart.DotLine), -1);
            // c.xAxis().setTitle("X-Axis Title Place Holder", "Arial Bold Italic", 12);
            // c.yAxis().setTitle("Y-Axis Title Place Holder", "Arial Bold Italic", 12);
            c.xAxis().setLabelStyle("Arial Bold");
            c.yAxis().setLabelStyle("Arial Bold");
            
            c.yAxis().setTickDensity(30);
            c.xAxis().setTickDensity(30);
            
            var l = c.addLineLayer(lineY, 0x000000);
            l.setXData(lineX);
            l.setLineWidth(3);

            ContourLayer layer = c.addContourLayer(dataX, dataY, dataZ);

            c.getPlotArea().moveGridBefore(layer);
            c.getPlotArea().moveGridBefore(l);

            ColorAxis cAxis = layer.setColorAxis(500, 60, Chart.TopLeft, 505, Chart.Right);
            
            cAxis.setTitle("Color Legend", "Arial Bold Italic", 12);
            cAxis.setLabelStyle("Arial Bold");
            viewer.Chart = c;
            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='<*cdml*><*font=bold*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}''");
        }
        
        
         public void CreateChart3D(WPFChartViewer viewer)
        {
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

            // Create a SurfaceChart object of size 720 x 600 pixels
            SurfaceChart c = new SurfaceChart(600, 600);

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
         
         
         
         private void WPFChart3D_MouseMoveChart(object sender, MouseEventArgs e)
         {
             WPFChart3D.updateViewPort(true, false);
             int mouseX = WPFChart3D.ChartMouseX;
             int mouseY = WPFChart3D.ChartMouseY;

             // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
             if (Mouse.LeftButton == MouseButtonState.Pressed)
             {
                 if (m_isDragging)
                 {
                     m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                     m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                     WPFChart3D.updateViewPort(true, false);
                 }
                 
                 m_lastMouseX = mouseX;
                 m_lastMouseY = mouseY;
                 m_isDragging = true;
             }
             
             
         }
         
         private void WPFChart3D_ViewPortChanged(object sender, WPFViewPortEventArgs e)
         {
             if (e.NeedUpdateChart)
                 CreateChart3D((WPFChartViewer)sender);
         }
         private void WPFChart3D_MouseUpChart(object sender, MouseEventArgs e)
         {
             m_isDragging = false;
             WPFChart3D.updateViewPort(true, false);
         }
    }
}
