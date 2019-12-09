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

namespace Ttime
{
    public partial class Form1 : Form
    {
        delegate void UpdateLabl();
        UpdateLabl ul;
        DateTime startTime,endTime;
        bool pause;


        string l1, l2, b1  = "Start";

        CancellationTokenSource source;
        CancellationToken token;
        
        public Form1()
        {
            InitializeComponent();

            ul = new UpdateLabl(UpdateLabeText);
 
        }
  
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
             
            StopTack();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

         
        private void button1_Click (object sender, EventArgs e)
        {
            if (b1 == "Start")
            {
                startTime = DateTime.Now;
                endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 30, 00);
                pause = false;
                b1 = "Update";
                UpdateLabeText();
                source = new CancellationTokenSource();
                token = source.Token;
                Task.Run(() =>
                {
                    TimeRunning();
                }, token);
            }
        }
 
        private void button2_Click(object sender, EventArgs e)
        {
            if (b1 == "Update")
            {
                pause = true;
                b1 = "Start";
                l1 = "00:00:00";
                l2 = "Time:00:30:00";
                UpdateLabeText();
                Console.Write("--");
                StopTack();
            }
        }


        public void TimeRunning()
        {
            while (!pause)
            {

                var curTime = DateTime.Now;
                var updateTime = curTime.Subtract(startTime);
                l1 = curTime.Subtract(startTime).ToString("T");
                 
                l2 = "Time:" + endTime.Subtract(curTime.Subtract(startTime)).ToString("s").Split('T')[1];

                Console.WriteLine(updateTime.TotalSeconds);
              
               Invoke(ul);
                 
                if (updateTime.TotalSeconds > 1800 )
                {
                    pause = true;
                    b1 = "Start";
                     Invoke(ul);
                    StopTack();
                    OverTime();
                }

            }
           
        }
        public void StopTack()
        {
            pause = true;
            if (!token.CanBeCanceled)
            {
                source.Cancel();
                source.Dispose();
            }
          
        }

        public void UpdateLabeText()
        {
            label2.Text = l1;
            label1.Text = l2;
            button1.Text = b1;
        }

        public void OverTime()
        {
            MessageBox.Show("OverTime...take a rest");
        }
       
    }
}
