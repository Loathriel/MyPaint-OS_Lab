using MyPaint_OS_8_.Instruments.Shapes;
using Rectangle = System.Drawing.Rectangle;

namespace MyPaint_OS_8_
{
    public partial class Form1 : Form
    {
        private Shape? shape = null;
        private Selection? copyBuffer = null;
        List<Shape> shapes = new List<Shape>();
        List<Shape> undoBuffer = new List<Shape>();
        string filename = string.Empty;
        bool changed = false;
        Button SelectedTool;
        Image original;

        private readonly int defaultWidth, defaultHeight;
        private readonly Color def, selected;
        private static void NotImplemented()
        {
            MessageBox.Show("Not yet implemented", "Error!");
        }
        public Form1()
        {
            InitializeComponent();
            defaultWidth = pictureBox.Width;
            defaultHeight = pictureBox.Height;
            original = new Bitmap(defaultWidth, defaultHeight);
            pictureBox.Image = new Bitmap(original);
            SelectedTool = LineButton;
            def = RectangleButton.BackColor;
            selected = LineButton.BackColor;
            
        }

        private void SaveToFile()
        {
            using Image bmp = new Bitmap(pictureBox.Image);
            bmp.Save(filename);
            ResetPanel(false);
        }

        private void ResetPanel(bool clearScreen = true)
        {
            shapes.Clear();
            undoBuffer.Clear();
            shape = null;
            changed = false;
            changeEnabledState();

            if (clearScreen)
            { 
                pictureBox.Image?.Dispose();
                pictureBox.Image = new Bitmap(defaultWidth, defaultHeight);
            }
            pictureBox.Refresh();
        }

        private Shape createShape(Point p)
        {
            var lineWidth = (int)LineWidth.Value;
            var pen = new Pen(LineColor.BackColor, lineWidth);
            var brush = new SolidBrush(FillColor.BackColor);

            var fillShapes = !checkBox1.Checked;

            if (SelectedTool == LineButton)
                return new Line(p, pen);
            if (SelectedTool == RectangleButton)
                return new Instruments.Shapes.Rectangle
                    (p, pen, brush, true, fillShapes);
            if (SelectedTool == EllipseButton)
                return new Elipse(p, pen, brush, true, fillShapes);
            if (SelectedTool == LassoButton)
                return new Selection(p);
            return new Line(p, pen);
        }

        private bool CallSaveDialog()
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return false;

            filename = saveFileDialog1.FileName;
            SaveToFile();
            return true;
        }

        private bool CanResetGraphics()
        {
            if (!changed)
                return true;

            var result = MessageBox.Show(
                "There are unsaved changed. Do you want to save them?",
                "Warning",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.No)
            {
                changed = false;
                return true;
            }

            if (filename != string.Empty)
                SaveToFile();

            return CallSaveDialog();
        }

        private void AddShape()
        {
            changed = true;
            shapes.Add(shape);
            undoBuffer.Clear();
            if (shape is Selection)
                MyPaint();
            shape = null;
            changeEnabledState();
        }

        private void ActivateButton(Button b)
        {
            if (b == SelectedTool)
                return;

            SelectedTool.BackColor = def;
            b.BackColor = selected;
            SelectedTool = b;
        }
        private void changeEnabledState()
        {
            undoToolStripMenuItem.Enabled = shapes.Count == 0? false : true;
            redoToolStripMenuItem.Enabled = undoBuffer.Count == 0 ? false : true;
        }

        private void MyPaint()
        {
            var img = new Bitmap(original);
            using var graphics = Graphics.FromImage(img);
            foreach (var shape in shapes)
                shape.Paint(graphics);
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
            pictureBox.Refresh();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            e.Cancel = !CanResetGraphics();
        }
    }
}