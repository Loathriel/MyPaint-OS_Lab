using MyPaint_OS_8_.Instruments.Shapes;

namespace MyPaint_OS_8_
{
    partial class Form1
    {
        private void GraphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape shape in shapes)
            {
                shape.Paint(e.Graphics);
            }
        }

        private void GraphicsPanel_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void GraphicsPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void GraphicsPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (shape is null || shape.IsCompleted)
                shape = createShape(e.Location);
        }

        private void GraphicsPanel_MouseMove(object sender, MouseEventArgs e)
        {
            PositionLabel.Text = $"{e.Location.X}; {e.Location.Y}";

            if (shape != null && !shape.IsCompleted)
            {
                GraphicsPanel.Refresh();
                shape.MouseMove(e, graphics);
            }
        }

        private void GraphicsPanel_MouseUp(object sender, MouseEventArgs e)
        {
            shape?.MouseUp(e, graphics);
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
            ResetPanel(new Bitmap(filename));
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
            GraphicsPanel.Refresh();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoBuffer.Count == 0)
                return;
            changed = true;
            var tempShape = undoBuffer[^1];
            undoBuffer.RemoveAt(undoBuffer.Count - 1);
            shapes.Add(tempShape);
            GraphicsPanel.Refresh();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotImplemented();
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
                GraphicsPanel.Refresh();
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
