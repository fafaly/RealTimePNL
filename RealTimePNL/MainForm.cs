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
            // fdate = "20150416";//test
            InitializeComponent();
            lbDate.Text = fdate;
            lbTime.Text = DateTime.Now.ToLongTimeString();
            comboBox1.Text = "5";

            timer1.Interval = 5 * 1000 * 60;
        }

        void RefreshData()
        {
            ReadRealTimeData();
            dgvMain.DataSource = mtable;
            dgvMain.Refresh();
            lbTime.Text = DateTime.Now.ToLongTimeString();
            fdate = DateTime.Today.ToString("yyyyMMdd");
            lbDate.Text = fdate;
        }

        void GetTotalPNL()
        {
            float pnl = 0;
            float lasthold = 0;
            float curhold = 0;
            float curcls = 0;
            for (int i = 0; i < mtable.Rows.Count; i++)
            {
                float lastcls = float.Parse(mtable.Rows[i][2].ToString());
                if (mtable.Rows[i]["Current px"].ToString() == "" || mtable.Rows[i]["Current px"].ToString() == "0")
                {
                    curcls = lastcls;
                }
                else
                {
                    curcls = float.Parse(mtable.Rows[i]["Current px"].ToString());
                }

                float shr = float.Parse(mtable.Rows[i][1].ToString());
                //pnl+=float.Parse(mtable.Rows[i][4].ToString());
                lasthold += shr * lastcls;
                curhold += shr * curcls;
            }
            pnl = curhold - lasthold;
            lbLastHold.Text = lasthold.ToString();
            lbCurHold.Text = curhold.ToString();
            lbPNL.Text = pnl.ToString();
            lbReturn.Text = Math.Round(pnl / curhold * 100, 3).ToString() + "%";
        }

        private void ReadRealTimeData()
        {
            for (int i = 0; i < mtable.Rows.Count; i++)
            {
                float lastcls = float.Parse(mtable.Rows[i][2].ToString());
                float cls = GetRealTimeDll.GetCurrentPx(mtable.Rows[i][0].ToString());
                if (cls == 0)
                {
                    cls = lastcls;
                }
                mtable.Rows[i]["Current Px"] = cls;//get current px
                mtable.Rows[i]["PNL"] = (cls - lastcls) * float.Parse(mtable.Rows[i][1].ToString());//get pnl

                mtable.Rows[i]["Return(%)"] = Math.Round((cls / lastcls - 1) * 100, 3);
            }
            GetTotalPNL();
        }

        private void ReadRealTimeDataCSV()
        {
            for (int i = 0; i < mtable.Rows.Count; i++)
            {
                string fname = REALTIMEPATH + fdate + "\\" + mtable.Rows[i][0] + ".csv";
                DataTable atable = csvhelper.OpenCSV(fname);
                float lastcls = float.Parse(mtable.Rows[i][2].ToString());
                float cls = 0;
                if (atable.Rows.Count != 0)
                {

                    cls = float.Parse(atable.Rows[atable.Rows.Count - 1][4].ToString()) / 10000;
                    if (cls == 0)
                    {
                        cls = lastcls;
                    }
                    mtable.Rows[i]["Current Px"] = cls;//get current px
                    mtable.Rows[i]["PNL"] = (cls - lastcls) * float.Parse(mtable.Rows[i][1].ToString());//get pnl
                }
                else
                {
                    cls = lastcls;
                    mtable.Rows[i]["Current Px"] = cls;
                    mtable.Rows[i]["PNL"] = 0;
                }
                mtable.Rows[i]["Return(%)"] = Math.Round((cls / lastcls - 1) * 100, 3);
            }
            GetTotalPNL();
        }

        /// <summary>
        /// Read the actHoldingBOD file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog opendialog = new OpenFileDialog();
            if (opendialog.ShowDialog() == DialogResult.OK)
            {
                mtable = csvhelper.OpenCSV(opendialog.FileName);
                int index = opendialog.FileName.IndexOf("production");
                string production = "production" + opendialog.FileName[index + 10];
                tabControl1.TabPages[0].Text = production;
                mtable.Rows[0].Delete();
                mtable.Columns.Add("Position", typeof(float));
                mtable.Columns.Add("Current px", typeof(float));
                mtable.Columns.Add("Return(%)", typeof(float));
                mtable.Columns.Add("PNL", typeof(float));
                for (int i = 0; i < mtable.Rows.Count; i++)
                {
                    float shr = float.Parse(mtable.Rows[i][1].ToString());
                    float cls = float.Parse(mtable.Rows[i][2].ToString());
                    mtable.Rows[i]["Position"] = (shr * cls).ToString();
                    if (mtable.Rows[i][0].ToString().Substring(0, 1) == "#")
                    {
                        mtable.Rows[i].Delete();
                    }
                }
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
            MessageBox.Show(this, "Copy right by xtcapital\n\n  2015/04/20  yliu", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
