namespace MyPaint_OS_8_.Instruments.Shapes
{
    internal class Img: Shape
    {
        Image image;
        public Img(Point start, Image image)
        {
            this.Start = start;
            this.image = new Bitmap(image);
        }
        public override void Paint(Graphics graphics)
        {
            graphics.DrawImageUnscaled(image, Start);
        }

    }
}
