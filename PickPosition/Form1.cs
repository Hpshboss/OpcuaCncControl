using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PickPosition
{
    public partial class Form1 : Form
    {
        static Thread PickThread = new Thread(PickPosition.Picking);
        public static string localPath;
        public static string textMessage = "Connecting...";
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(PickThread.IsAlive == false)
            {
                timer1.Enabled = true;
                localPath = textBox1.Text;
                PickPosition.flagflag = 1;
                PickThread.Start();
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PickPosition.flagflag = 0;
            PickThread.Interrupt();
            textMessage = "Finish";
            ResetThread();
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = textMessage;
        }

        public static void ResetThread()
        {
            PickThread = new Thread(PickPosition.Picking);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
