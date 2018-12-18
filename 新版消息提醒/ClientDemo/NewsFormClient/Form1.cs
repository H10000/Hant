using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewsFormClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void bindMessage(int state, string typeid)
        {

            panelmessagelist.Controls.Clear();
            DataTable dt = ws.GetMessageList("BAYNEXUS", personnel_id, state, typeid, ref ret);//0是全部，1是未读，2是已读
            int ddd = dt.Rows.Count;
            int d = ddd / 5;
            int f = ddd % 5;
            int n = 1;
            for (int i = 0; i < ddd; i++)
            {
                panelmessagelist.Controls.Add(panlm(n, flag, height, dt.Rows[i]["c_sendStatus"].ToString(), dt.Rows[i]["typename"].ToString(), dt.Rows[i]["t_alert_time"].ToString(), dt.Rows[i]["c_detail"].ToString(), dt.Rows[i]["c_alert_url"].ToString(), dt.Rows[i]["c_m_id"].ToString()));
            }
            panelmessagelist.VerticalScroll.Visible = true;//竖的
            panelmessagelist.HorizontalScroll.Visible = false;//横的
        }

        public Panel panlm(int n, bool flag, int height, string state, string leibie, string time, string detil, string url, string messageid)
        {
            var pan1 = new System.Windows.Forms.Panel();
            // pan1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            pan1.Location = new System.Drawing.Point(0, height);
            pan1.Name = "panelm_" + n;
            pan1.Size = new System.Drawing.Size(660, 70);
            pan1.TabIndex = 1;
            pan1.Visible = flag;
            // 
            // ckb_1
            // 
            var ckb_1 = new System.Windows.Forms.CheckBox();
            ckb_1.AutoSize = true;
            ckb_1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            ckb_1.ForeColor = System.Drawing.Color.Red;
            ckb_1.Location = new System.Drawing.Point(10, 10);
            ckb_1.Name = "ckb_" + n;
            ckb_1.Size = new System.Drawing.Size(75, 21);
            ckb_1.TabIndex = 0;
            ckb_1.Text = "【" + state.Replace("2", "未读").Replace("3", "未读").Replace("5", "已读") + "】";
            ckb_1.UseVisualStyleBackColor = true;
            ckb_1.Tag = messageid;
            // 
            // labed_1
            // 
            var labed_1 = new System.Windows.Forms.Label();
            labed_1.AutoSize = true;
            labed_1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labed_1.Location = new System.Drawing.Point(82, 10);
            labed_1.Name = "labed_" + n;
            labed_1.Size = new System.Drawing.Size(34, 60);
            labed_1.TabIndex = 1;
            labed_1.Text = leibie;

            // 
            // labtime_1
            // 
            var labtime_1 = new System.Windows.Forms.Label();
            labtime_1.AutoSize = true;
            labtime_1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labtime_1.Location = new System.Drawing.Point(564, 12);
            labtime_1.Name = "labtime_1";
            labtime_1.Size = new System.Drawing.Size(76, 17);
            labtime_1.TabIndex = 3;
            labtime_1.Text = Convert.ToDateTime(time).ToString("MM-dd HH:mm");
            // 
            // labcontent_1
            // 
            var labcontent_1 = new System.Windows.Forms.Label();
            labcontent_1.AutoEllipsis = true;
            labcontent_1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labcontent_1.Location = new System.Drawing.Point(82, 38);
            labcontent_1.Name = "labcontent_1";
            labcontent_1.Size = new System.Drawing.Size(501, 23);
            labcontent_1.TabIndex = 4;
            labcontent_1.Text = detil;
            // 
            // lineShape2
            // 
            var lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            lineShape2.BorderColor = System.Drawing.Color.Silver;
            lineShape2.Name = "lineShape_" + n;
            lineShape2.X1 = 3;
            lineShape2.X2 = 650;
            lineShape2.Y1 = 65;
            lineShape2.Y2 = 65;
            // 
            // shapeContainer2
            // 
            var shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            shapeContainer2.Location = new System.Drawing.Point(0, 0);
            shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            shapeContainer2.Name = "shapeContainer_" + n;
            shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            lineShape2});
            shapeContainer2.Size = new System.Drawing.Size(650, 70);
            shapeContainer2.TabIndex = 5;
            shapeContainer2.TabStop = false;

            // 
            // labqianwang_1
            // 
            var labqianwang_1 = new System.Windows.Forms.Label();
            labqianwang_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            labqianwang_1.AutoSize = true;
            labqianwang_1.Cursor = System.Windows.Forms.Cursors.Hand;
            labqianwang_1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            labqianwang_1.Location = new System.Drawing.Point(589, 38);
            labqianwang_1.Name = "labqianwang_" + n;
            labqianwang_1.Size = new System.Drawing.Size(44, 17);
            labqianwang_1.TabIndex = 6;
            labqianwang_1.TabStop = true;
            //labqianwang_1.Text = "前往》";
            labqianwang_1.Tag = url;
            labqianwang_1.Click += new System.EventHandler(labMessage_Click);
            pan1.Controls.Add(labqianwang_1);
            pan1.Controls.Add(labcontent_1);
            pan1.Controls.Add(labtime_1);

            pan1.Controls.Add(labed_1);
            pan1.Controls.Add(ckb_1);
            pan1.Controls.Add(shapeContainer2);

            return pan1;
        }
    }
}
