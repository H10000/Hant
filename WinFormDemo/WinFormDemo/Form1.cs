using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //TreeViewColumn cl1 = new TreeViewColumn();
            //cl1.HeaderText = "树";
            //dataGridView1.Columns.Add(cl1);
            //DataGridViewComboBoxExColumn cl2 = new DataGridViewComboBoxExColumn();
            //cl2.HeaderText = "下拉框";
            //dataGridView1.Columns.Add(cl2);

            DataTable table = new DataTable();
            table.Columns.Add("AAA", typeof(DateTime));
            table.Columns.Add("BBB", typeof(Decimal));
            table.Columns.Add("CCC", typeof(String));

            string[] tb2 = { "北京","南京","111"};
            DataGridViewComboBoxExColumn col3 =new DataGridViewComboBoxExColumn();
            col3.DataSource = tb2;
            col3.Width = 200;
            dataGridView1.Columns.Add(col3);
            for (int i = 0; i < 3; i++)
            {
                DataRow row = table.NewRow();

                row["AAA"] = DateTime.Now;
                row["BBB"] = 123456789.1234567;
                row["CCC"] = 2;

                table.Rows.Add(row);
            }
            table.AcceptChanges();

            CalendarColumn col4 = new CalendarColumn();
            dataGridView1.Columns.Add(col4);
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = table;

        }
    }
}
