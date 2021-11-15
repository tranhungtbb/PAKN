using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TLI.FakeImage
{
    public partial class LearningConfirmDialog : Form
    {
        public bool IsFake { get { return this.radioFake.Checked;  }  }
        public LearningConfirmDialog()
        {
            InitializeComponent();
        }
    }
}
