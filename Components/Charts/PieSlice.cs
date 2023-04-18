using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace LogAnalysisTool.Components.Charts
{
    public class PieSlice : Shape
    {
        private static readonly DependencyPropertyKey CenterAngleKey =
            DependencyProperty.RegisterReadOnly("CenterAngle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty CenterProperty =
            EllipseGeometry.CenterProperty.AddOwner(typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty SweepAngleProperty =
            DependencyProperty.Register("SweepAngle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        private readonly ArcSegment _arcSegment = new();
        private readonly LineSegment _lineSegment = new();
        private readonly PathFigure _pathFigure = new();
        private readonly PathGeometry _pathGeometry = new();

        public PieSlice()
        {
            _pathFigure.IsClosed = true;
            _pathFigure.Segments.Add(_lineSegment);
            _pathFigure.Segments.Add(_arcSegment);
            _pathGeometry.Figures.Add(_pathFigure);
        }

        private void OnGeometryPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _pathFigure.StartPoint = Center;

            double angle = Math.PI * StartAngle / 180;
            double x = Center.X + Radius * Math.Sin(angle);
            double y = Center.Y - Radius * Math.Cos(angle);
            _lineSegment.Point = new Point(x, y);

            angle = Math.PI * (StartAngle + SweepAngle) / 180;
            x = Center.X + Radius * Math.Sin(angle);
            y = Center.Y - Radius * Math.Cos(angle);
            _arcSegment.Point = new Point(x, y);
            _arcSegment.Size = new Size(Radius, Radius);
            _arcSegment.IsLargeArc = SweepAngle > 180;
            _arcSegment.SweepDirection = SweepDirection.Clockwise;

            CenterAngle = StartAngle + SweepAngle / 2;

            InvalidateMeasure();
        }

        private static void OnGeometryPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as PieSlice).OnGeometryPropertyChanged(args);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return new Size(Center.X + Radius, Center.Y + Radius);
        }

        protected override Geometry DefiningGeometry => _pathGeometry;

        public Point Center { set { SetValue(CenterProperty, value); } get { return (Point)GetValue(CenterProperty); } }

        public double CenterAngle { set { SetValue(CenterAngleKey, value); } get { return (double)GetValue(CenterAngleProperty); } }

        public double Radius { set { SetValue(RadiusProperty, value); } get { return (double)GetValue(RadiusProperty); } }

        public double StartAngle { set { SetValue(StartAngleProperty, value); } get { return (double)GetValue(StartAngleProperty); } }

        public double SweepAngle { set { SetValue(SweepAngleProperty, value); } get { return (double)GetValue(SweepAngleProperty); } }

        public static readonly DependencyProperty CenterAngleProperty =
            CenterAngleKey.DependencyProperty;
    }
}
