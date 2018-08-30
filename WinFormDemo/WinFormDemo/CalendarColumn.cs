using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WinFormDemo
{
    /// <summary>
    /// 日期栏位控件
    /// </summary>
    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn()
            : base(new CalendarCell())
        {
        }
        /// <summary>
        /// 获取或设置用于创建新单元格的模板。
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                //确认单元格使用了自定义的日期单元格模版
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                   // throw new InvalidCastException(WinFormDemo.Properties.Resources.Message);
                }
                base.CellTemplate = value;
            }
        }
    }
    /// <summary>
    /// 日期单元格
    /// </summary>
    public class CalendarCell : DataGridViewTextBoxCell
    {
        public CalendarCell()
            : base()
        {
            // 默认为短日期型
            this.Style.Format = "d";
        }
        /// <summary>
        /// 初始化用于编辑单元格的控件。
        /// </summary>
        /// <param name="rowIndex">单元格所在位置的从零开始的行索引。</param>
        /// <param name="initialFormattedValue">它表示在开始编辑时单元格显示的值。</param>
        /// <param name="dataGridViewCellStyle">表示单元格样式的 System.Windows.Forms.DataGridViewCellStyle。</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            //初始化之后设置编辑状态的值
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
            if (Value != null && Value != System.DBNull.Value)
                ctl.Value = Convert.ToDateTime(this.Value);
        }
        /// <summary>
        /// 获取单元格的寄宿编辑控件的类型。
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(CalendarEditingControl);
            }
        }
        /// <summary>
        /// 获取或设置单元格中值的数据类型。
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(DateTime);
            }
        }
        /// <summary>
        /// 获取新记录所在行中单元格的默认值。
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
    /// <summary>
    /// 日期单元格控件的编辑状态
    /// </summary>
    internal class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            this.Format = DateTimePickerFormat.Short;
        }

        /// <summary>
        /// 实现接口 IDataGridViewEditingControl.EditingControlFormattedValue 
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.Value = DateTime.Parse(newValue);
                }
            }
        }

        /// <summary>
        /// 检索单元格的格式化值。
        /// </summary>
        /// <param name="context">System.Windows.Forms.DataGridViewDataErrorContexts 值的按位组合，它指定需要数据的上下文。</param>
        /// <returns>一个 System.Object，表示单元格内容的格式化版本。</returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        /// <summary>
        /// 更改控件的用户界面 (UI)，使之与指定单元格样式一致。
        /// </summary>
        /// <param name="dataGridViewCellStyle">要用作用户界面的模型的 System.Windows.Forms.DataGridViewCellStyle。</param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// 获取或设置该承载单元格的父行的索引。包含该单元格的行的索引，如果没有父行，则为 -1。
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        /// <summary>
        /// 确定指定的键是应由编辑控件处理的常规输入键，还是应由 System.Windows.Forms.DataGridView 处理的特殊键。
        /// </summary>
        /// <param name="key">一个 System.Windows.Forms.Keys，表示按下的键。</param>
        /// <param name="dataGridViewWantsInputKey">当 System.Windows.Forms.DataGridView 要处理 keyData 中的 System.Windows.Forms.Keys时，则为 true；否则为 false。</param>
        /// <returns>如果指定的键是应由编辑控件处理的常规输入键，则为 true；否则为 false。</returns>
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 准备当前选中的单元格以进行编辑。
        /// </summary>
        /// <param name="selectAll">为 true，则选择单元格的全部内容；否则为 false。</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            //暂时空
        }

        /// <summary>
        /// 获取或设置一个值，该值指示每当值更改时，是否需要重新定位单元格的内容。
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取或设置包含单元格的 System.Windows.Forms.DataGridView。
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示编辑控件的值是否与承载单元格的值不同。
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        /// <summary>
        /// 获取当鼠标指针位于 System.Windows.Forms.DataGridView.EditingPanel 上方但不位于编辑控件上方时所使用的光标。
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        /// <summary>
        /// 重写DateTimePicker事件
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            //DataGridView中Cell的Changed事件 
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}



