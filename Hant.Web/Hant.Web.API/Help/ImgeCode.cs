using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Hant.Web.API.Help
{
    public static class ImgeCode
    {
        //生成验证码图片
        private static Image CreateCheckCodeImage(string checkCode)
        {
            //若验证码为空，则直接返回
            //if (checkCode == null || checkCode.Trim() == string.Empty)
            //    return new Image();
            //根据验证码的长度确定输出图片的宽度
            int iWidth = (int)Math.Ceiling(checkCode.Length * 15m);
            int iHeight = 20;
            //创建图像
            Bitmap image = new Bitmap(iWidth, iHeight);
            //从图像获取一个绘图面
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random r = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线10条
                for (int i = 0; i < 10; i++)
                {
                    int x1 = r.Next(image.Width);
                    int x2 = r.Next(image.Width);
                    int y1 = r.Next(image.Height);
                    int y2 = r.Next(image.Height);
                    //用银色画出噪音线
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                //画图片的前景噪音点50个
                for (int i = 0; i < 50; i++)
                {
                    int x = r.Next(image.Width);
                    int y = r.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(r.Next()));
                }
                //画图片的框线
                g.DrawRectangle(new Pen(Color.SaddleBrown), 0, 0, image.Width - 1, image.Height - 1);
                //定义绘制文字的字体
                Font f = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                //线性渐变画刷
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Purple, 1.2f, true);
                g.DrawString(checkCode, f, brush, 2, 2);

                ////创建内存流用于输出图片
                //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                //{
                //    //图片格式制定为png
                //    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //    //清除缓冲区流中的所有输出
                //    Response.ClearContent();
                //    //输出流的HTTP MIME类型设置为"image/Png"
                //    Response.ContentType = "image/Png";
                //    //输出图片的二进制流
                //    Response.BinaryWrite(ms.ToArray());
                //}
                return image;
            }
            finally
            {
                //释放Bitmap对象和Graphics对象
                g.Dispose();
                image.Dispose();
            }
        }
    }
}