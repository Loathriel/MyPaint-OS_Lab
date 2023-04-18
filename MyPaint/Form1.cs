using MyPaint_OS_8_.Instruments.Shapes;

namespace MyPaint_OS_8_
{
    public partial class Form1 : Form
    {
        private Shape? shape = null;
        Graphics graphics;
        List<Shape> shapes = new List<Shape>();
        List<Shape> undoBuffer = new List<Shape>();
        string filename = string.Empty;
        bool changed = false;
        int defaultWidth, defaultHeight;
        Button SelectedTool;
        Color def, selected;
        private static void NotImplemented()
        {
            MessageBox.Show("Not yet implemented", "Error!");
        }
        public Form1()
        {
            InitializeComponent();
            defaultWidth = GraphicsPanel.Width;
            defaultHeight = GraphicsPanel.Height;
            GraphicsPanel.Image = new Bitmap(defaultWidth, defaultHeight);
            graphics = GraphicsPanel.CreateGraphics();
            SelectedTool = LineButton;
            def = RectangleButton.BackColor;
            selected = LineButton.BackColor;
        }

        private void SaveToFile()
        {
            Image bmp = new Bitmap(GraphicsPanel.Width, GraphicsPanel.Height);
            Graphics g = Graphics.FromImage(bmp);
            foreach (Shape shape in shapes)
                shape.Paint(g);
            bmp.Save(filename);
            changed = false;
            bmp.Dispose();
        }

        private void ResetPanel(Image? Image = null)
        {
            shapes.Clear();
            graphics.Clear(Color.Transparent);

            if (Image == null)
            {
                GraphicsPanel.Width = defaultWidth;
                GraphicsPanel.Height = defaultHeight;
            }
            else
            {
                GraphicsPanel.Width = Image.Width;
                GraphicsPanel.Height = Image.Height;
                shape = new Img(new Point(0, 0), Image);
                AddShape();
                Image.Dispose();
            }
            GraphicsPanel.Image = new Bitmap(GraphicsPanel.Width, GraphicsPanel.Height);

        }

        private Shape createShape(Point p)
        {
            var lineWidth = (int)LineWidth.Value;
            var pen = new Pen(LineColor.BackColor, lineWidth);
            var brush = new SolidBrush(FillColor.BackColor);

            if (SelectedTool == LineButton)
                return new Line(p, pen);
            if (SelectedTool == RectangleButton)
                return new Instruments.Shapes.Rectangle
                    (p, pen, brush, true, true);
            if (SelectedTool == EllipseButton)
                return new Elipse(p, pen, brush, true, true);
            if (SelectedTool == LassoButton)
                return new Line(p, pen);

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
            if (changed)
            {
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

                else if (filename == string.Empty)
                    return CallSaveDialog();
            }
            return true;
        }

        private void AddShape()
        {
            changed = true;
            shapes.Add(shape);
            GraphicsPanel.Refresh();
            undoBuffer.Clear();
            shape = null;
        }

        private void ActivateButton(Button b)
        {
            if (b == SelectedTool)
                return;

            SelectedTool.BackColor = def;
            b.BackColor = selected;
            SelectedTool = b;
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