using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program
{
    public partial class GetNameForm : Form
    {
        public GetNameForm()
        {
            InitializeComponent();
        }

        private void GetNameForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        public string getFragmentName()
        {
            return textBox1.Text;
        }
    }
}
