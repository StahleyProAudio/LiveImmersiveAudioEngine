using LIAE.AH.SQ5;
using System.Globalization;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LiveImmersiveAudioEngine
{
    public partial class Form1 : Form
    {

        private int _micCount = 0;
        private SQTcpClient? _sqClient;
        private bool _dragging = false;
        private Point _dragOffset;   // mouse position within the picturebox at MouseDown

        private Point _dragStart; // mouse position inside icoMic1 at drag
        private SQ5Controller _sq5Controller;

        public Form1()
        {
            InitializeComponent();
        }

        private void IcoMic1_Paint(object sender, PaintEventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                using (Pen pen = new Pen(Color.LightGray, 2)) // light grey, 2px thick
                {
                    // Draw rectangle inside the PictureBox bounds
                    Rectangle rect = new Rectangle(0, 0, pic.Width - 1, pic.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private async void btnAddInput_Click(object sender, EventArgs e)
        {

            if (byte.TryParse(
                    txtSelectedChannel.Text,
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture,
                    out byte channel))
            {

                // make sure that the SQ starts with the default pan of center (50%) for the new input, so that it doesn't jump when you drag it for the first time.
                var panner = new SQPanner(0);
                var sq = new SQ5Controller(channel, panner);
                await sq.RefreshPanAsync();

                var pb = new PictureBox
                {
                    Name = $"pbMic_{_micCount}",
                    Image = Properties.Resources.MicrophoneIcon,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(48, 48),
                    Location = new Point((this.ClientSize.Width - 48) / 2, (this.ClientSize.Height - 48) / 2),
                    Cursor = Cursors.SizeAll, // visual hint: draggable
                    BackColor = Color.Transparent
                };

                MakeDraggable(pb);

                // Click event
                pb.Click += Pb_Click;

                // Drag events
                pb.MouseDown += Pb_MouseDown;
                pb.MouseMove += Pb_MouseMove;
                pb.MouseUp += Pb_MouseUp;

                this.Controls.Add(pb);
                pb.BringToFront();
            }
            else
            {
                MessageBox.Show(
                    "Please enter a valid number.",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtSelectedChannel.Focus();
                txtSelectedChannel.SelectAll();
                return;
            }
        }

        // --- Drag support (click + drag to move the PictureBox) ---
        private void MakeDraggable(Control control)
        {
            bool dragging = false;
            Point dragStart = Point.Empty;

            control.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                dragging = true;
                dragStart = e.Location;              // offset inside the control
                control.Capture = true;
            };

            control.MouseMove += (s, e) =>
            {
                if (!dragging || e.Button != MouseButtons.Left) return;

                // Move control by delta (keeps cursor position consistent while dragging)
                control.Left = e.X + control.Left - dragStart.X;
                control.Top = e.Y + control.Top - dragStart.Y;
            };

            control.MouseUp += (s, e) =>
            {
                dragging = false;
                control.Capture = false;
            };
        }

        private void Pb_Click(object? sender, EventArgs e)
        {
            var pb = (PictureBox)sender!;
            // Example: select/highlight, show properties, etc.
            pb.BorderStyle = pb.BorderStyle == BorderStyle.None ? BorderStyle.FixedSingle : BorderStyle.None;
        }

        private void Pb_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            _dragging = true;
            _dragOffset = e.Location;   // where inside the PB you clicked
        }

        private void Pb_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_dragging || e.Button != MouseButtons.Left) return;

            var pb = (PictureBox)sender!;

            // we need to update the SQ with the new Pan value based on the new position of the PictureBox.
            var val14 = (ClientSize.Width <= pb.Width)
                ? 0
                : (int)Math.Round(pb.Left * 100.0 / (ClientSize.Width - pb.Width) / SQ5Config.PAN_MAX);

            // the number on the label is the channel number
            var selectedPictureBox = sender as PictureBox;
            string name = selectedPictureBox.Name;




            // var sq = new SQ5Controller(0, new SQPanner(val14)); // TODO: need to get the correct input index for this PictureBox




            // Move relative to its current position (no jumping)
            pb.Left += e.X - _dragOffset.X;
            pb.Top += e.Y - _dragOffset.Y;
        }

        private void Pb_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _dragging = false;
        }
    }
}
