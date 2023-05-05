using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogAnalysisTool.Components.Charts
{
    /// <summary>
    /// PieSlice.cs by Charles Petzold, June 2009 https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/september/charting-with-datatemplates
    /// </summary>
    public class PieSlice : Shape
    {
        private static readonly DependencyPropertyKey _centerAngleKey =
            DependencyProperty.RegisterReadOnly("CenterAngle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty _centerProperty =
            EllipseGeometry.CenterProperty.AddOwner(typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty _radiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty _startAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice), new FrameworkPropertyMetadata(OnGeometryPropertyChanged));
        public static readonly DependencyProperty _sweepAngleProperty =
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

        public Point Center { set { SetValue(_centerProperty, value); } get { return (Point)GetValue(_centerProperty); } }

        public double CenterAngle { set { SetValue(_centerAngleKey, value); } get { return (double)GetValue(_centerAngleProperty); } }

        public double Radius { set { SetValue(_radiusProperty, value); } get { return (double)GetValue(_radiusProperty); } }

        public double StartAngle { set { SetValue(_startAngleProperty, value); } get { return (double)GetValue(_startAngleProperty); } }

        public double SweepAngle { set { SetValue(_sweepAngleProperty, value); } get { return (double)GetValue(_sweepAngleProperty); } }

        public static readonly DependencyProperty _centerAngleProperty =
            _centerAngleKey.DependencyProperty;
    }
}
