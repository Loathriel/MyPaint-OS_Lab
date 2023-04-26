using System.Drawing;

namespace MyPaint_OS_8_.Instruments.Shapes
{
    internal class Selection: Shape
    {
        Bitmap image;
        Point startedFrom, newStart;
        System.Drawing.Rectangle hitbox;
        bool finishedSelecting = false, fillWithWhite = true;
        public Selection(Point start)
        {
            this.Start = start;
            this.End = start;
            this.newStart = start;
            this.pen = new Pen(Color.Black);
            this.pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.brush = new SolidBrush(Color.White);
            this.hitbox = new System.Drawing.Rectangle();
        }
        public Selection(Point start, Point end):
            this(start)
        {
            this.End = end;
            FinishSelecting();
        }
        private Selection(Selection copy)
        {
            this.image = copy.image;
            this.hitbox = copy.hitbox;
            this.Start = this.newStart = new Point(0, 0);
            this.hitbox.Location = this.Start;
            this.End = new Point(image.Width, image.Height);
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
        private void FinishSelecting()
        {
            int startX = Math.Min(End.X, Start.X),
                startY = Math.Min(End.Y, Start.Y),
                endX = Math.Max(End.X, Start.X),
                endY = Math.Max(End.Y, Start.Y);

            newStart = Start = new Point(startX, startY);
            End = new Point(endX, endY);

            hitbox.Location = Start;
            hitbox.Width = endX - startX;
            hitbox.Height = endY - startY;

            finishedSelecting = true;
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
                hitbox.X = newStart.X - startedFrom.X + args.X;
                hitbox.Y = newStart.Y - startedFrom.Y + args.Y;
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
                newStart = hitbox.Location;
                this.Paint(graphics);
                return;
            }

            End = args.Location;

            FinishSelecting();
            image = image.Clone(hitbox, image.PixelFormat);
        }

        public override void Paint(Graphics graphics)
        {
            int startX = Math.Min(Start.X, End.X),
                startY = Math.Min(Start.Y, End.Y),
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
                graphics.DrawRectangle(pen, hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height);
        }

        public Selection Copy()
            => new Selection(this);

        public void ForceComplete()
        {
            completed = true;
        }

        public void ClockwiseRotation(Graphics graphics)
        {
            Rotate(RotateFlipType.Rotate90FlipNone, graphics);
        }
        public void AntiClockwiseRotation(Graphics graphics)
        {
            Rotate(RotateFlipType.Rotate90FlipXY, graphics);
        }
        private void Rotate(RotateFlipType rotation, Graphics graphics)
        {
            image.RotateFlip(rotation);
            (hitbox.Width, hitbox.Height) = (hitbox.Height, hitbox.Width);
            Paint(graphics);
        }
    }
}
