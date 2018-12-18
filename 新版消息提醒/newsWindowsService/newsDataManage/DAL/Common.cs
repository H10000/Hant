using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

namespace newsDataManage.DAL
{
    public class Common
    {
        /// <summary>
        /// 字符转时间，默认最小时间
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public DateTime StrToDateTime_N(string _str)
        {
            try
            {
                if (!string.IsNullOrEmpty(_str))
                {
                    return Convert.ToDateTime(_str);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// 字符转整数
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public int StrToInt_N(string _str)
        {
            try
            {
                if (!string.IsNullOrEmpty(_str))
                {
                    return Convert.ToInt32(_str);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取配置xml节点值
        /// </summary>
        /// <param name="NodePath"></param>
        /// <returns></returns>
        public string Get_SetXmlNode(string NodePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFilePath = System.Windows.Forms.Application.StartupPath + @"\config.xml";
            xmlDoc.Load(xmlFilePath);
            XmlNode retNode = xmlDoc.SelectSingleNode(NodePath);
            return retNode.InnerText;
        }

        //动态调用 WebService
        //客户端动态调用代码
        public object Transfer_WebService(string Url, string parmClass, string parmMethod, object[] Parameters)
        {
            try
            {
                // 1. 使用 WebClient 下载 WSDL 信息。
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(Url);
                // 2. 创建和格式化 WSDL 文档。
                ServiceDescription description = ServiceDescription.Read(stream);
                // 3. 创建客户端代理代理类。
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                importer.ProtocolName = "Soap"; // 指定访问协议。
                importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档。
                // 4. 使用 CodeDom 编译客户端代理类。
                CodeNamespace nmspace = new CodeNamespace(); // 为代理类添加命名空间，缺省为全局空间。
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nmspace);
                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters parameter = new CompilerParameters();
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = true;
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
                // 5. 使用 Reflection 调用 WebService。
                if (!result.Errors.HasErrors)
                {
                    Assembly asm = result.CompiledAssembly;
                    Type t = asm.GetType(parmClass); // 如果在前面为代理类添加了命名空间，此处需要将命名空间添加到类型前面。
                    object o = Activator.CreateInstance(t);
                    MethodInfo method = t.GetMethod(parmMethod);
                    return method.Invoke(o, Parameters).ToString();
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
