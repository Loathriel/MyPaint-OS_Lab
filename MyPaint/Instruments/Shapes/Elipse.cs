namespace MyPaint_OS_8_.Instruments.Shapes
{
    internal class Elipse : Shape
    {
        public Elipse(Point start, Pen pen, Brush brush, bool DrawOutline, bool DrawSolid)
        {
            this.Start = start;
            this.End = start;
            this.pen = pen;
            this.brush = brush;
            this.DrawOutline = DrawOutline;
            this.DrawSolid = DrawSolid;
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
            int startX = Math.Min(Start.X, End.X),
                startY = Math.Min(Start.Y, End.Y),
                width = Math.Abs(Start.X - End.X),
                height = Math.Abs(Start.Y - End.Y);
            if (DrawSolid)
                graphics.FillEllipse(brush, startX, startY, width, height);
            if (DrawOutline)
                graphics.DrawEllipse(pen, startX, startY, width, height);
        }
    }
}
