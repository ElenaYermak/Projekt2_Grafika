﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ptojekt2_Yermak
{
    public partial class Form1 : Form
    {
        MyLoopTime loop = new MyLoopTime();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _3D _3d = new _3D(pictureBox1);
            loop = new MyLoopTime();
            loop.Load(_3d);
            loop.Run();
        }
    }
}
