using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;

namespace PosKafe
{
    public partial class Form_QuanLyKho : MetroForm
    {
        public Form_QuanLyKho()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
        }

        private void Form_QuanLyKho_Load(object sender, EventArgs e)
        {

        }
    }
}
