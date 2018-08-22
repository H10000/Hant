using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Hant.Web.API.Helper
{
    public static class ImgeHelper
    {
        /// <summary>
        /// 得到单个字符
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string getOneCode(int a, int b)
        {
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            char c = (char)ran.Next(a, b);
            return c.ToString();
        }

        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <returns></returns>
        public static string getCode()
        {
            string code = "";
            //Ascii码范围：数字:48-57，小写字母:97-122，大写字母:65-90  
            code += getOneCode(48, 57);
            code += getOneCode(97, 122);
            code += getOneCode(65, 90);
            //code += getOneCode(97, 122);
            code += getOneCode(48, 57);
            return code;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static Image CreateCheckCodeImage(string checkCode)
        {
            //若验证码为空，则直接返回
            //if (checkCode == null || checkCode.Trim() == string.Empty)
            //    return new Image();
            //根据验证码的长度确定输出图片的宽度
            int iWidth = (int)Math.Ceiling(checkCode.Length * 30m);
            int iHeight = 30;
            //创建图像
            Bitmap image = new Bitmap(iWidth, iHeight);
            //从图像获取一个绘图面
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线10条
                for (int i = 0; i < 10; i++)
                {
                    int x1 = new Random(Guid.NewGuid().GetHashCode()).Next(image.Width);
                    int x2 = new Random(Guid.NewGuid().GetHashCode()).Next(image.Width);
                    int y1 = new Random(Guid.NewGuid().GetHashCode()).Next(image.Height);
                    int y2 = new Random(Guid.NewGuid().GetHashCode()).Next(image.Height);
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
                Font f = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                //线性渐变画刷
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Purple, 1.2f, true);
                int location_x = 2;
                foreach (char c in checkCode)
                {
                    g.DrawString(c.ToString(), f, brush, location_x, 2);
                    location_x += 30;
                }
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
                ////释放Bitmap对象和Graphics对象
                //g.Dispose();
                //image.Dispose();
            }
        }

        /// <summary>
        ///根据图片文件的路径使用文件流打开，并保存为byte[] 
        /// </summary>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        public static byte[] GetPictureData(string imagepath)
        {
            //根据图片文件的路径使用文件流打开，并保存为byte[] 
            FileStream fs = new FileStream(imagepath, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }
        /// <summary>
        /// 二进制转图片
        /// </summary>
        /// <param name="streamByte"></param>
        /// <returns></returns>
        public static System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        //图片转为base64编码的字符串  
        public static string ImgToBase64String(Image Imagefile)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefile);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //base64编码的字符串转为图片  
        public static Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                bmp.Save(@"d:\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save(@"d:\"test.bmp", ImageFormat.Bmp);  
                //bmp.Save(@"d:\"test.gif", ImageFormat.Gif);  
                //bmp.Save(@"d:\"test.png", ImageFormat.Png);  
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}