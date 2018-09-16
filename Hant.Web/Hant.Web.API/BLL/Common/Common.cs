using Hant.Web.API.DAL;
using Hant.Web.API.DAL.Entity;
using Hant.Web.API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hant.Web.API.BLL.Common
{
    public class Common
    {
        UnitOfWork db = new UnitOfWork();
        public string GetUserID()
        {
            DateTime dtNow = DateTime.Now;
            int Year = dtNow.Year;
            int Month = dtNow.Month;
            int Day = dtNow.Day;
            int Hour = dtNow.Hour;
            int Minute = dtNow.Minute;
            int Number = 1;
            sys_code_number item = new sys_code_number();
            item.Year = Year;
            item.Month = Month;
            item.Day = Day;
            item.Hour = Hour;
            item.Minute = Minute;
            item.Mark = (int)CodeMark.UserID;
            item.Number = Number;
            IEnumerable<sys_code_number> ls = db.Sys_code_numberRepository.Get(fliter: e => e.Year == Year
                                                                                       && e.Month == Month && e.Day == Day && e.Hour == Hour
                                                                                       && e.Minute == Minute && e.Mark == (int)CodeMark.UserID
                                                                                       , orderBy: e => e.OrderByDescending(u => u.Number));
            if (ls.Count() > 0)
            {
                Number = 1 + Convert.ToInt32(ls.FirstOrDefault().Number);
                item.Number = Number;
            }
            db.Sys_code_numberRepository.Insert(item);
            db.Save();
            return "P" + Convert.ToString(Year) + Convert.ToString(Month).PadLeft(2, '0') + Convert.ToString(Day).PadLeft(2, '0')
                   + Convert.ToString(Hour).PadLeft(2, '0') + Convert.ToString(Minute).PadLeft(2, '0') + Convert.ToString(Number).PadLeft(4, '0');
        }
    }
}