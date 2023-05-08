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

            List<Pair> x = calculation.GradientDecent();
            // var joinedNames = x.Aggregate((a, b) => a.ToString() + b.ToString());
            string s = "";
            for (int i = 0; i < x.Count; i++) {
                s += x[i].ToString() + "\n";
            }
            TestTextBox.Text = calculation.Result().ToString();

            createChart(WPFChart);

        }

        private Calculation _calculation = new();


        //Name of demo module
        public string getName() { return "Contour Chart"; }


        public int getNoOfCharts() { return 1; }


        public void createChart(WPFChartViewer viewer) {
            double[] dataX = _calculation.GetL().ToArray();
            double[] dataY = _calculation.GetS().ToArray();


            double[] dataZ = new double[dataX.Length * dataY.Length];
            for (int yIndex = 0; yIndex < dataY.Length; ++yIndex) {
                double y = dataY[yIndex];
                for (int xIndex = 0; xIndex < dataX.Length; ++xIndex) {
                    double x = dataX[xIndex];
                    double t = Math.Pow((x - y), 2) + Math.Pow((x + y - 10), 2) / 9;
                    dataZ[yIndex * dataX.Length + xIndex] = t < 12 ? t : double.NaN;
                }
            }
            
            List<Pair> points = _calculation.GradientDecent();
            double[] lineX = new double[points.Count];
            double[] lineY = new double[points.Count];

            for (int i = 0; i < points.Count; i++) {
                lineX[i] = points[i].X;
                lineY[i] = points[i].Y;
            }


            XYChart c = new XYChart(600, 500);


            c.addTitle("z = x * sin(y) + y * sin(x)      ", "Arial Bold Italic", 15);

  
            c.setPlotArea(75, 40, 400, 400, -1, -1, -1, c.dashLineColor(unchecked((int)0x80000000),
                Chart.DotLine), -1);


            c.xAxis().setTitle("X-Axis Title Place Holder", "Arial Bold Italic", 12);
            c.yAxis().setTitle("Y-Axis Title Place Holder", "Arial Bold Italic", 12);


            c.xAxis().setLabelStyle("Arial Bold");
            c.yAxis().setLabelStyle("Arial Bold");


            c.yAxis().setTickDensity(40);
            c.xAxis().setTickDensity(40);

            

            //layer.addDataSet(lineY, 0xffffff);
            var l = c.addLineLayer(lineY, 0x000000);
            l.setXData(lineX);
            l.setLineWidth(3);

            ContourLayer layer = c.addContourLayer(dataX, dataY, dataZ);

            c.getPlotArea().moveGridBefore(layer);
            c.getPlotArea().moveGridBefore(l);


            ColorAxis cAxis = layer.setColorAxis(505, 40, Chart.TopLeft, 400, Chart.Right);


            cAxis.setTitle("Color Legend Title Place Holder", "Arial Bold Italic", 12);
            
            
            cAxis.setLabelStyle("Arial Bold");


            viewer.Chart = c;


            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='<*cdml*><*font=bold*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}''");
        }
    }
}
