using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormDemo
{
    //设计一个继承自ComboBox的下拉框编辑列组件
    public class DataGridViewComboBoxExEditingControl : ComboBox, IDataGridViewEditingControl
    {
        protected int rowIndex;
        protected DataGridView dataGridView;
        protected bool valueChanged = false;
        private ArrayList m_list = new ArrayList();

        protected override void OnEnter(EventArgs e)
        {
            m_list.Clear();
            m_list.AddRange(this.Items);
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.Items.Clear();
            this.Items.AddRange(m_list.ToArray());
            base.OnLeave(e);
        }
        protected override void OnTextUpdate(EventArgs e)
        {
            this.Items.Clear();
            foreach (object o in this.m_list)
            {
                if (GetChineseSpell(o.ToString()).ToLower().Contains(this.Text.ToLower()) || o.ToString().Contains(this.Text.ToLower()))
                {
                    this.Items.Add(o);
                }
            }
            this.DroppedDown = true;
            this.Cursor = Cursors.Default;//保持光标形状
            this.SelectionStart = this.Text.Length;//光标保持在文字后面
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextUpdate(e);
        }
        //private void NotifyDataGridViewOfValueChange()
        //{
        //    valueChanged = true;
        //}

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
            // get { return Cursors.IBeam; }
        }

        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }

        public object EditingControlFormattedValue
        {
            set
            {
                Text = value.ToString();
            }
            get
            {
                return this.Text;
            }

        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {

            switch (key & Keys.KeyCode)
            {
                //case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                //case Keys.Right:
                case Keys.Enter:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                case Keys.LButton:
                    return true;
                default:
                    return false;
            }
        }
        
        protected override void OnSelectedItemChanged(EventArgs eventargs)
        {
            //DataGridView中Cell的Changed事件 
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnSelectedItemChanged(eventargs);
        }
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            //if (selectAll)
            //{
            //    SelectAll();
            //}
            //else
            //{
            //    this.SelectionStart = this.ToString().Length;
            //}
        }
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public int EditingControlRowIndex
        {
            get { return this.rowIndex; }
            set { this.rowIndex = value; }
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { this.valueChanged = value; }
        }
        static public string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        static public string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else
                return cnChar;
        }
    }

    //定制该扩展列的单元格
    public class DataGridViewComboBoxExCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            DataGridViewComboBoxExEditingControl clt = DataGridView.EditingControl as DataGridViewComboBoxExEditingControl;

            DataGridViewComboBoxExColumn col = (DataGridViewComboBoxExColumn)OwningColumn;
            clt.Items.Clear();
            clt.Items.AddRange(col.DataSource);//不能用DataSource，因为绑定数据之后就不行对Items执行clear和add事件；另外，绑定数据后会默认选择第一行，如果需要输入多个字符，这样会产生全选，覆盖的输入效果。
            clt.AutoCompleteMode = AutoCompleteMode.Suggest;
            //clt.FormattingEnabled = true;
            clt.Text = Convert.ToString(this.Value);
        }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewComboBoxExEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return "";
            }
        }
    }

    //定制该扩展列
    public class DataGridViewComboBoxExColumn : DataGridViewColumn
    {
        public DataGridViewComboBoxExColumn()
            : base(new DataGridViewComboBoxExCell())
        {
        }
        private object[] dataSoruce = null;

        public object[] DataSource
        {
            get { return dataSoruce; }
            set { dataSoruce = value; }
        }
        private string valueMember;

        public string ValueMember
        {
            get { return valueMember; }
            set { valueMember = value; }
        }
        private string displayMember;

        public string DisplayMember
        {
            get { return displayMember; }
            set { displayMember = value; }
        }


        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewComboBoxExCell)))
                {
                    throw new InvalidCastException("is not DataGridViewComboxExCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}


