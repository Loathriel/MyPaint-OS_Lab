namespace MyPaint_OS_8_.Instruments.Shapes
{
    internal class Line : Shape
    {
        public Line(Point start, Pen pen)
        {
            this.Start = start;
            this.End = start;
            this.pen = pen;
        }
        public override void MouseMove(MouseEventArgs args, Graphics graphics)
        {
            base.MouseMove(args, graphics);
            End = args.Location;
            this.Paint(graphics);
        }
        public override void MouseUp(MouseEventArgs args, Graphics graphics)
        {
            base.MouseUp(args, graphics);
            End = args.Location;
            completed = true;
            this.Paint(graphics);
        }
        public override void Paint(Graphics graphics)
        {
            graphics.DrawLine(pen, Start, End);
        }
    }
}
