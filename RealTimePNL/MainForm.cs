using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RealTimePNL
{
    public partial class MainForm : Form
    {
      

        CSVFileHelper csvhelper = new CSVFileHelper();

        string fdate = DateTime.Today.ToString("yyyyMMdd");

        const string REALTIMEPATH = "D:\\data\\TICKDATA\\MarketDataL2\\";

        //Main forms table
        DataTable mtable = new DataTable();

        //Timer bindtime = new Timer();

        public MainForm()
        {
            fdate = "20150417";//test
            InitializeComponent();
            lbDate.Text = fdate;
            lbTime.Text = DateTime.Now.ToLongTimeString();
            comboBox1.Text = "5";

            timer1.Interval = 5 * 1000 *60;
        }

        void RefreshData()
        {
            ReadRealTimeData();
            dgvMain.DataSource = mtable;
            dgvMain.Refresh();
            lbTime.Text = DateTime.Now.ToLongTimeString();
        }


        private void ReadRealTimeData()
        { 
            for(int i = 0;i < mtable.Rows.Count;i++)
            {
                string fname = REALTIMEPATH + fdate + "\\" + mtable.Rows[i][0] + ".csv";
                DataTable atable = csvhelper.OpenCSV(fname);
                if (atable.Rows.Count != 0)
                {
                    float cls = float.Parse(atable.Rows[atable.Rows.Count - 1][4].ToString()) / 10000;
                    mtable.Rows[i][2] = cls.ToString();
                    mtable.Rows[i][3] = cls * float.Parse(mtable.Rows[i][1].ToString());//get pnl
                }
            }
        }

        /// <summary>
        /// Read the actHoldingBOD file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog opendialog = new OpenFileDialog();
            if(opendialog.ShowDialog()==DialogResult.OK)
            {              
                mtable = csvhelper.OpenCSV(opendialog.FileName);
                int index = opendialog.FileName.IndexOf("production");
                string production = "production" + opendialog.FileName[index + 10];
                tabControl1.TabPages[0].Text = production;
                mtable.Rows[0].Delete();
                mtable.Columns.Add("pnl", typeof(float));
            }
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(comboBox1.Text) * 1000 * 60;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dgvMain.Rows.Count != 0)
            {
                this.RefreshData();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,"Copy right by xtcapital\n\n  2015/04/20  yliu","About",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
