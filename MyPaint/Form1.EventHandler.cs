using MyPaint_OS_8_.Instruments.Shapes;

namespace MyPaint_OS_8_
{
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
            if (shape != null)
                AddShape();
            shape = copyBuffer?.Copy();
            using var graphics = pictureBox.CreateGraphics();
            shape?.Paint(graphics);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
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
    }
}
