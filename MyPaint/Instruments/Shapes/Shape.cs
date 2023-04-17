namespace MyPaint_OS_8_.Instruments.Shapes
{
    abstract class Shape
    {
        protected bool completed = false;
        protected Point Start, End;
        protected Brush brush;
        protected Pen pen;
        protected bool DrawOutline, DrawSolid;

        public bool IsCompleted
            => completed;
        public virtual void MouseDown(MouseEventArgs args, Graphics graphics) { }
        public virtual void MouseMove(MouseEventArgs args, Graphics graphics) { }
        public virtual void MouseUp(MouseEventArgs args, Graphics graphics) { }
        public virtual void MouseClick(MouseEventArgs args, Graphics graphics) { }
        public virtual void MouseDoubleClick(MouseEventArgs args, Graphics graphics) { }
        public abstract void Paint(Graphics graphics);
    }
}
