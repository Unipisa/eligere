using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PollingStation
{
    public partial class Form1 : Form
    {
        private Button b;
        public Form1()
        {
            InitializeComponent();
        }

        private void B_Click(object sender, EventArgs e)
        {
            MessageBox.Show("La procedura di voto avverrà in un ambiente privato il cui stato sarà eliminato\nquando la finestra nera di edge sarà chiusa.\n\nRicordare di chiudere la finestra al termine dell'operazione di voto.", "Avviso importante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "--start-maximized --inprivate https://eligere.unipi.it/eligere");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var p = new Panel();
            p.Dock = DockStyle.Fill;
            b = new Button() { Text = "Inizia la procedura di voto", Width=300, Height=200 };
            b.Click += B_Click;
            p.Controls.Add(b);
            Controls.Add(p);
            this.Left = 0;
            this.Top = 0;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.FromControl(this).Bounds;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (b != null)
            {
                b.Left = (this.Width - b.Width) / 2;
                b.Top = (this.Height - b.Height) / 2;
            }

        }
    }
}
