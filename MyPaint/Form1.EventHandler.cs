using AForge.Imaging.Filters;
using MyPaint_OS_8_.Instruments.Shapes;

namespace MyPaint_OS_8_
{
    #region Dummy
    class DummyClass { }
    #endregion
    partial class Form1
    {
        private void Graphics_Paint(object sender, PaintEventArgs e)
        {
            if (shape is Selection)
                shape.Paint(e.Graphics);
        }

        private void Graphics_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Graphics_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void Graphics_MouseDown(object sender, MouseEventArgs e)
        {
            if (shape is Selection)
            {
                using var graphics = pictureBox.CreateGraphics();
                shape.MouseDown(e, graphics);
            }

            else if (shape is null || shape.IsCompleted)
                shape = createShape(e.Location);
        }

        private void Graphics_MouseMove(object sender, MouseEventArgs e)
        {
            PositionLabel.Text = $"{e.X}; {e.Y}";

            if (shape is Selection && shape.IsCompleted)
            {

            }

            if (shape != null && !shape.IsCompleted)
            {
                pictureBox.Refresh();
                using var graphics = pictureBox.CreateGraphics();
                shape.MouseMove(e, graphics);
            }
        }

        private void Graphics_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics graphics;
            if (shape is Selection selection)
            {
                graphics = pictureBox.CreateGraphics();
                if (!selection.IsSelected)
                    selection.SetImage((Bitmap)pictureBox.Image);
            }
            else
                graphics = Graphics.FromImage(pictureBox.Image);
            shape?.MouseUp(e, graphics);
            graphics.Dispose();
            if (shape != null && shape.IsCompleted)
                AddShape();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CanResetGraphics())
            {
                filename = string.Empty;
                ResetPanel();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanResetGraphics())
                return;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            filename = openFileDialog1.FileName;
            using var pic = new Bitmap(filename);
            original = new Bitmap(pic);
            pictureBox.Image = new Bitmap(original);
            ResetPanel(false);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == string.Empty)
                CallSaveDialog();
            else
                SaveToFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CallSaveDialog();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shapes.Count == 0)
                return;
            changed = true;
            var tempShape = shapes[^1];
            shapes.RemoveAt(shapes.Count - 1);
            undoBuffer.Add(tempShape);
            MyPaint();
            changeEnabledState();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoBuffer.Count == 0)
                return;
            changed = true;
            var tempShape = undoBuffer[^1];
            undoBuffer.RemoveAt(undoBuffer.Count - 1);
            shapes.Add(tempShape);
            MyPaint();
            changeEnabledState();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shape is Selection selection)
                copyBuffer = selection;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shape is Selection selection)
                selection.ForceComplete();
            if (shape != null)
                AddShape();
            shape = copyBuffer?.Copy();
            using var graphics = pictureBox.CreateGraphics();
            shape?.Paint(graphics);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shape is Selection selection)
                selection.ForceComplete();
            if (shape != null)
                AddShape();
            shape = new Selection(new Point(0, 0), new Point(pictureBox.Width, pictureBox.Height));
            ((Selection)shape).SetImage((Bitmap)pictureBox.Image);
        }

        private void pasteFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }

        private void LineColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = LineColor.BackColor;
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            LineColor.BackColor = colorDialog1.Color;
        }

        private void FillColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = FillColor.BackColor;
            if (colorDialog1.ShowDialog() != DialogResult.OK)
                return;
            FillColor.BackColor = colorDialog1.Color;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                shape = null;
                pictureBox.Refresh();
                return;
            }
        }

        private void LineButton_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
        }

        private void RectangleButton_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
        }

        private void EllipseButton_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
        }

        private void LassoButton_Click(object sender, EventArgs e)
        {
            ActivateButton((Button)sender);
        }

        private void RotateClockwiseButton_Click(object sender, EventArgs e)
        {
            if (shape is Selection selection)
            {
                pictureBox.Refresh();
                using var graphics = pictureBox.CreateGraphics();
                selection.ClockwiseRotation(graphics);
                return;
            }
            var img = pictureBox.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox.Image = img;
            changed = true;
        }

        private void RotateAntiClockwiseButton_Click(object sender, EventArgs e)
        {
            if (shape is Selection selection)
            {
                pictureBox.Refresh();
                using var graphics = pictureBox.CreateGraphics();
                selection.AntiClockwiseRotation(graphics);
                return;
            }
            var img = pictureBox.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipXY);
            pictureBox.Image = img;
            changed = true;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var newImg = ((Bitmap)pictureBox.Image).Clone(
                new RectangleF(0, 0, pictureBox.Image.Width, pictureBox.Image.Height), 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var img = new Invert().Apply(newImg);
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
        }

        private void edgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Edges().ApplyInPlace((Bitmap)pictureBox.Image);
            pictureBox.Refresh();
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var img = new Grayscale(0.2125, 0.7154, 0.0721).Apply((Bitmap)pictureBox.Image);
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
        }

        private void oilFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OilPainting().ApplyInPlace((Bitmap)pictureBox.Image);
            pictureBox.Refresh();
        }
    }
}
