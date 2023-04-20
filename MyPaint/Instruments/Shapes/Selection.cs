namespace MyPaint_OS_8_.Instruments.Shapes
{
    internal class Selection: Shape
    {
        Bitmap image;
        Point startedFrom, newStart, newEnd, currentStart, currentEnd;
        System.Drawing.Rectangle hitbox;
        bool finishedSelecting = false, fillWithWhite = true;
        public Selection(Point start)
        {
            this.Start = start;
            this.End = start;
            this.newStart = start;
            this.newEnd = start;
            this.currentStart = start;
            this.currentEnd = start;
            this.pen = new Pen(Color.Black);
            this.pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.brush = new SolidBrush(Color.White);
            this.hitbox = new System.Drawing.Rectangle();
        }
        private Selection(Selection copy)
        {
            this.image = copy.image;
            this.hitbox = copy.hitbox;
            this.Start = this.currentStart = this.newStart = new Point(0, 0);
            this.hitbox.Location = this.Start;
            this.End = this.currentEnd = this.newEnd = new Point(image.Width, image.Height);
            this.pen = copy.pen;
            this.fillWithWhite = false;
            this.finishedSelecting = true;
            this.completed = false;
        }
        public bool IsSelected => finishedSelecting;
        public void SetImage(Bitmap full)
        {
            image = new Bitmap(full);
        }
        public override void MouseDown(MouseEventArgs args, Graphics graphics)
        {
            base.MouseDown(args, graphics);

            if (finishedSelecting)
                if (!hitbox.Contains(args.Location))
                {
                    completed = true;
                    return;
                }
                startedFrom = args.Location;
        }
        public override void MouseMove(MouseEventArgs args, Graphics graphics)
        {
            base.MouseMove(args, graphics);
            if (args.Button == MouseButtons.None)
                return;

            if (finishedSelecting)
            {
                currentStart.X = newStart.X - startedFrom.X + args.X;
                currentStart.Y = newStart.Y - startedFrom.Y + args.Y;
                currentEnd.X = newEnd.X - startedFrom.X + args.X;
                currentEnd.Y = newEnd.Y - startedFrom.Y + args.Y;
                hitbox.Location = currentStart;
            }
            else
                End = args.Location;
            this.Paint(graphics);
        }
        public override void MouseUp(MouseEventArgs args, Graphics graphics)
        {
            base.MouseUp(args, graphics);

            if (finishedSelecting)
            {
                newStart = currentStart;
                newEnd = currentEnd;
                this.Paint(graphics);
                return;
            }

            newEnd = End = args.Location;

            int startX = Math.Min(End.X, Start.X),
                startY = Math.Min(End.Y, Start.Y),
                endX = Math.Max(End.X, Start.X),
                endY = Math.Max(End.Y, Start.Y);

            currentStart = newStart = Start = new Point(startX, startY);
            currentEnd = newEnd = End = new Point(endX, endY);

            hitbox.Location = Start;
            hitbox.Width = endX - startX;
            hitbox.Height = endY - startY;

            finishedSelecting = true;
            image = image.Clone(hitbox, image.PixelFormat);
        }

        public override void Paint(Graphics graphics)
        {
            int startX = Math.Min(Start.X, End.X),
                startY = Math.Min(Start.Y, End.Y),
                newStartX = Math.Min(currentStart.X, currentEnd.X),
                newStartY = Math.Min(currentStart.Y, currentEnd.Y),
                width = Math.Abs(Start.X - End.X),
                height = Math.Abs(Start.Y - End.Y);

            if (finishedSelecting)
            {
                if (fillWithWhite)
                    graphics.FillRectangle(brush, startX, startY, width, height);
                graphics.DrawImage(image, hitbox);
            }

            if (!finishedSelecting)
                graphics.DrawRectangle(pen, startX, startY, width, height);
            else if (!IsCompleted)
                graphics.DrawRectangle(pen, newStartX, newStartY, width, height);
        }

        public Selection Copy()
            => new Selection(this);

        public void ForceComplete()
        {
            completed = true;
        }

    }
}
