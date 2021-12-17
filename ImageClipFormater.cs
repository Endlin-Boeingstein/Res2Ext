using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

//建立i元件引用类
class ImageClipFormater
{
    //创建acf实例
    public AnimateClipFormater acf = new AnimateClipFormater();
    //定义文件名称
    public string pid=null;
    //建立imgMapper的JSON对象
    public JObject imgMapper = new JObject();
    //新功能更新而停用//建立animMapper的JSON对象
    //新功能更新而停用///public JObject animMapper = new JObject();
    //生成i元件引用部分并对受损i元件和a元件进行修复
    public void ImageClipFormat(string Fpath, JArray Rja, JObject ext)
    {
        try
        {
            //创建路径文件夹实例
            DirectoryInfo TheFolder = new DirectoryInfo(Fpath);
            //遍历文件夹内文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                //新功能更新而停用///if (NextFile.Extension == "xml")
                //新功能更新而停用///{
                //新功能更新而停用//给文件名称赋值
                //新功能更新而停用///this.id = NextFile.Name;
                //新功能更新而停用///}
                //新功能更新而停用///else { }
                //流式读取文件类型
                FileStream stream = new FileStream(NextFile.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);
                string fileclass = "";
                try
                {
                    for (int i = 0; i < 2; i++)
                    {
                        fileclass += reader.ReadByte().ToString();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                stream.Close();
                //判定是否为xml
                if (fileclass == "6068")
                {
                    //获取DOMBitmapInstance节点libraryItemName属性
                    //创建xml读取对象
                    XmlDocument xmlDoc = new XmlDocument();
                    //读取xml
                    xmlDoc.Load(NextFile.FullName);
                    //判定是i元件还是a元件
                    if (NextFile.Name.Substring(0, 1) == "i"|| NextFile.Name.Substring(0, 1) == "M")
                    {
                        //读取DOMBitmapInstance节点
                        //新功能更新而停用，用了报错///XmlElement element = (XmlElement)xmlDoc.SelectSingleNode("DOMSymbolItem/timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMBitmapInstance");
                        XmlElement element = (XmlElement)xmlDoc.GetElementsByTagName("DOMBitmapInstance")[0];
                        //读取DOMBitmapInstance节点libraryItemName属性
                        string name = element.GetAttribute("libraryItemName");
                        //遍历resources数组
                        foreach (var item in Rja)
                        {
                            //判定是否为atlas，用以排除
                            if (!item.ToString().Contains("atlas"))
                            {
                                //读取各个切图的id
                                string id = ((JObject)item)["id"].ToString();
                                //读取各个切图的path
                                JArray path = (JArray)item["path"];
                                //得到path最后一个元素，即i元件引用的切图名称
                                string pid = path[path.Count - 1].ToString();
                                if (pid == name)
                                {
                                    //添加i元件切图引用数据
                                    imgMapper.Add(new JProperty(NextFile.Name.Substring(0, NextFile.Name.Length - 4), id));
                                    break;
                                }
                                else continue;
                            }
                            else { }
                        }


                            //修复a和d的值
                            //读取matrix节点
                            //新功能更新而停用，用了报错///XmlElement melement = (XmlElement)xmlDoc.SelectSingleNode("DOMSymbolItem/timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMBitmapInstance/matrix/Matrix");
                            XmlElement melement = (XmlElement)xmlDoc.GetElementsByTagName("Matrix")[0];
                        //设定ma和md
                        string ma = null, md = null;
                        //获得a属性字符串
                        ma = melement.GetAttribute("a");
                        //获得d属性字符串
                        md = melement.GetAttribute("d");
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数//读取matrix节点a属性
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数///double a = double.Parse(melement.GetAttribute("a"));
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数//读取matrix节点d属性
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数///double d = double.Parse(melement.GetAttribute("d"));
                        //判断a或者d是否存在
                        if (ma == null || md == null || (ma == "0" && md == "0") || ma == "" || md == "")
                        {
                            //设a为0.781250
                            melement.SetAttribute("a", "0.781250");
                            //设d为0.781250
                            melement.SetAttribute("d", "0.781250");
                            //保存xml
                            xmlDoc.Save(NextFile.FullName);
                        }
                    }
                    if (NextFile.Name.Substring(0, 1) == "a" || NextFile.Name.Substring(0, 1) == "A")
                    {
                        //读取matrix节点
                        //新功能更新而停用，用了报错///XmlElement melement = (XmlElement)xmlDoc.SelectSingleNode("DOMSymbolItem/timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMSymbolInstance/matrix/Matrix");
                        XmlElement melement = (XmlElement)xmlDoc.GetElementsByTagName("Matrix")[0];
                        //设定ma和md
                        string ma = null, md = null;
                        //获得a属性字符串
                        ma = melement.GetAttribute("a");
                        //获得d属性字符串
                        md = melement.GetAttribute("d");
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数//读取matrix节点a属性
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数///double a = double.Parse(melement.GetAttribute("a"));
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数//读取matrix节点d属性
                        //新功能更新而停用,并且a和d就是字符串而不是双精度浮点数///double d = double.Parse(melement.GetAttribute("d"));
                        //判断a或者d是否存在
                        if (ma == null || md == null || (ma == "0" && md == "0")|| ma == ""|| md == "")
                        {
                            //设a为1.000000
                            melement.SetAttribute("a", "1.000000");
                            //设d为1.000000
                            melement.SetAttribute("d", "1.000000");
                            //保存xml
                            xmlDoc.Save(NextFile.FullName);
                        }
                        //建立a元件引用类
                        acf.AnimateClipFormat(NextFile.Name);
                    }
                    else { }
                }
                else { }
            }
            //新功能更新而停用，用了报错//在imgSz后增加imgMapper数组
            //新功能更新而停用，用了报错///ext.Property("imgSz").AddAfterSelf(new JProperty("imgMapper", imgMapper));
            //新功能更新而停用，用了报错//创建成员便于传输ext
            //新功能更新而停用，用了报错///public var iext = ext;
        }
        catch
        {
            Console.WriteLine("ImageClipFormat ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }
}

