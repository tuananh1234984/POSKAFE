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
    public partial class Form_QuanLyMenu : MetroForm
    {
        public Form_QuanLyMenu()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
        }

        private void Form_QuanLyMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
