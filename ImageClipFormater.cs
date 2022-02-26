﻿using Newtonsoft.Json;
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
    //此功能因画蛇添足而移除//建立imgSz的输出成员
    //此功能因画蛇添足而移除///public JObject ImgSz;
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
                    //判断为SPCUtil解析的元件类型并提示
                    if (NextFile.Name.Substring(0, 1) == "M" || NextFile.Name.Substring(0, 1) == "A")
                    {
                        Console.WriteLine("抱歉，不支持用SPCUtil解析PAM得到的元件");
                    }
                    //判断为TwinKles-ToolKit解析的元件类型并提示
                    if (NextFile.Name.Substring(0, 2) == "sp" || NextFile.Name.Substring(0, 2) == "an")
                    {
                        Console.WriteLine("抱歉，不支持用TwinKles-ToolKit解析PAM得到的元件");
                    }
                    //判定是i元件还是a元件
                    if (NextFile.Name.Substring(0, 1) == "i")
                    {
                        //预置matrix节点
                        XmlElement matrix = xmlDoc.CreateElement("matrix", xmlDoc.DocumentElement.NamespaceURI);
                        //预置Matrix节点
                        XmlElement Matrix = xmlDoc.CreateElement("Matrix", xmlDoc.DocumentElement.NamespaceURI);
                        //设a为1.000000
                        Matrix.SetAttribute("a", "1.000000");
                        //设d为1.000000
                        Matrix.SetAttribute("d", "1.000000");
                        //将Matrix作为matrix的子节点
                        matrix.PrependChild(Matrix);
                        //新功能更新而停用//移除matrix的xmlns属性
                        //新功能更新而停用///matrix.RemoveAttribute("xmlns");
                        //读取DOMBitmapInstance节点
                        //新功能更新而停用，用了报错///XmlElement element = (XmlElement)xmlDoc.SelectSingleNode("DOMSymbolItem/timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMBitmapInstance");
                        XmlElement element = (XmlElement)xmlDoc.GetElementsByTagName("DOMBitmapInstance")[0];
                        //判断该i元件是否引用位图
                        if (element == null)
                        {
                            Console.WriteLine(NextFile.Name.Substring(0, NextFile.Name.Length - 4) + "元件未引用位图，将会引发错误");
                        }
                        else
                        {
                            //读取DOMBitmapInstance节点，检查i元件是否引用位图
                            foreach (XmlElement el in xmlDoc.GetElementsByTagName("DOMBitmapInstance"))
                            {
                                //判断是否写引用位图名
                                if (el.GetAttribute("libraryItemName") == "" || el.GetAttribute("libraryItemName") == null || el == null)
                                {
                                    Console.WriteLine(NextFile.Name.Substring(0, NextFile.Name.Length - 4) + "元件未写引用位图名，将会引发错误");
                                }
                                else
                                {
                                    //判断是否存在matrix，默认为0
                                    int matrixtrue = 0;
                                    foreach (XmlElement mat in el.ChildNodes)
                                    {
                                        if (mat.Name == "matrix")
                                        {
                                            matrixtrue = 1;
                                        }
                                        else { }
                                    }
                                    if (matrixtrue == 0)
                                    {
                                        el.PrependChild(matrix);
                                        //保存xml
                                        xmlDoc.Save(NextFile.FullName);
                                    }
                                    else { }
                                }
                            }
                            //重新读取xml
                            xmlDoc.Load(NextFile.FullName);
                            //重新读取DOMBitmapInstance节点
                            element = (XmlElement)xmlDoc.GetElementsByTagName("DOMBitmapInstance")[0];
                            //读取DOMBitmapInstance节点libraryItemName属性
                            string name = element.GetAttribute("libraryItemName");
                            //新功能更新而停用//记录id用于资源引用部分size重写的变量
                            //新功能更新而停用///string rid = null;
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
                                        //新功能更新而停用//记录id用来对资源引用部分size进行重写
                                        //新功能更新而停用///rid = id;
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
                            if (ma == null || md == null || ma == "0" || md == "0" || ma == "" || md == "")
                            {
                                //a不存在
                                if (ma == null || ma == "0" || ma == "")
                                {
                                    //设a为1.000000
                                    melement.SetAttribute("a", "1.000000");
                                }
                                //d不存在
                                if (md == null || md == "0" || md == "")
                                {
                                    //设d为1.000000
                                    melement.SetAttribute("d", "1.000000");
                                }
                                else { }
                                //保存xml
                                xmlDoc.Save(NextFile.FullName);
                            }
                            else { }

                            //重新获得a属性字符串
                            ma = melement.GetAttribute("a");
                            //重新获得d属性字符串
                            md = melement.GetAttribute("d");
                            //此功能因画蛇添足而移除//对资源引用部分size进行重写
                            //此功能因画蛇添足而移除//转换x缩放率类型
                            //此功能因画蛇添足而移除///double a = double.Parse(ma);
                            //此功能因画蛇添足而移除//转换y缩放率类型
                            //此功能因画蛇添足而移除///double d = double.Parse(md);
                            //此功能因画蛇添足而移除//建立imgSz的JObject类
                            //此功能因画蛇添足而移除///JObject imgSz = (JObject)ext["imgSz"];
                            //此功能因画蛇添足而移除//将对应的size转换为json数组
                            //此功能因画蛇添足而移除//新功能更新而停用///JArray sz = (JArray)imgSz[rid];
                            //此功能因画蛇添足而移除//遍历resources数组
                            //此功能因画蛇添足而移除///foreach (var item in Rja)
                            //此功能因画蛇添足而移除///{
                            //此功能因画蛇添足而移除//判定是否为atlas，用以排除
                            //此功能因画蛇添足而移除///if (!item.ToString().Contains("atlas"))
                            //此功能因画蛇添足而移除///{
                            //此功能因画蛇添足而移除//读取各个切图的id
                            //此功能因画蛇添足而移除///string id = ((JObject)item)["id"].ToString();
                            //此功能因画蛇添足而移除//读取各个切图的path
                            //此功能因画蛇添足而移除///JArray path = (JArray)item["path"];
                            //此功能因画蛇添足而移除//得到path最后一个元素，即i元件引用的切图名称
                            //此功能因画蛇添足而移除///string pid = path[path.Count - 1].ToString();
                            //此功能因画蛇添足而移除///if (pid == name)
                            //此功能因画蛇添足而移除///{
                            //此功能因画蛇添足而移除//读取各个切图的宽度并转换
                            //此功能因画蛇添足而移除///int width = (int)((int.Parse(((JObject)item)["aw"].ToString()) + 0.5 / a) * a);
                            //此功能因画蛇添足而移除//读取各个切图的高度并转换
                            //此功能因画蛇添足而移除///int height = (int)((int.Parse(((JObject)item)["ah"].ToString()) + 0.5 / d) * d);
                            //此功能因画蛇添足而移除//建立尺寸数组
                            //此功能因画蛇添足而移除///JArray sizearray = new JArray();
                            //此功能因画蛇添足而移除//写入数组内容
                            //此功能因画蛇添足而移除//写入宽度
                            //此功能因画蛇添足而移除///sizearray.Add(width);
                            //此功能因画蛇添足而移除//写入高度
                            //此功能因画蛇添足而移除///sizearray.Add(height);
                            //此功能因画蛇添足而移除//添加切图大小数据
                            //此功能因画蛇添足而移除///imgSz[id] = sizearray;
                            //此功能因画蛇添足而移除///break;
                            //此功能因画蛇添足而移除///}
                            //此功能因画蛇添足而移除///else continue;
                            //此功能因画蛇添足而移除///}
                            //此功能因画蛇添足而移除///else { }
                            //此功能因画蛇添足而移除///}
                            //此功能因画蛇添足而移除//为输出重写后的资源引用类赋值
                            //此功能因画蛇添足而移除///ImgSz = imgSz;
                            //保存xml
                            xmlDoc.Save(NextFile.FullName);
                        } 
                    }
                    //判断为a元件并修复
                    if (NextFile.Name.Substring(0, 1) == "a")
                    {
                        //预置matrix节点
                        XmlElement matrix = xmlDoc.CreateElement("matrix", xmlDoc.DocumentElement.NamespaceURI);
                        //预置Matrix节点
                        XmlElement Matrix = xmlDoc.CreateElement("Matrix", xmlDoc.DocumentElement.NamespaceURI);
                        //设a为1.000000
                        Matrix.SetAttribute("a", "1.000000");
                        //设d为1.000000
                        Matrix.SetAttribute("d", "1.000000");
                        //将Matrix作为matrix的子节点
                        matrix.PrependChild(Matrix);
                        //读取节点DOMSymbolInstance
                        XmlElement delement = (XmlElement)xmlDoc.GetElementsByTagName("DOMSymbolInstance")[0];
                        //判断该a元件是否引用元件
                        if (delement == null)
                        {
                            Console.WriteLine(NextFile.Name.Substring(0, NextFile.Name.Length - 4) + "元件未引用元件，将会引发错误");
                        }
                        else
                        {
                            //递归检测是否有matrix未修复
                            for (int i=0;i<xmlDoc.GetElementsByTagName("DOMSymbolInstance").Count;)
                            {
                                //读取DOMSymbolInstance节点，检查a元件是否引用元件
                                foreach (XmlElement el in xmlDoc.GetElementsByTagName("DOMSymbolInstance"))
                                {
                                    //判断是否写引用元件名
                                    if (el.GetAttribute("libraryItemName") == "" || el.GetAttribute("libraryItemName") == null || el == null)
                                    {
                                        Console.WriteLine(NextFile.Name.Substring(0, NextFile.Name.Length - 4) + "元件未写引用元件名，将会引发错误");
                                    }
                                    else
                                    {
                                        //判断是否存在matrix，默认为0
                                        int matrixtrue = 0;
                                        foreach (XmlElement mat in el.ChildNodes)
                                        {
                                            if (mat.Name == "matrix")
                                            {
                                                matrixtrue = 1;
                                                i++;
                                            }
                                            else { }
                                        }
                                        if (matrixtrue == 0)
                                        {
                                            el.PrependChild(matrix);
                                            //保存xml
                                            xmlDoc.Save(NextFile.FullName);
                                        }
                                        else { }
                                    }
                                }
                                //检测是否所有matrix都修复完毕，否则重置计数器
                                if (i < xmlDoc.GetElementsByTagName("DOMSymbolInstance").Count)
                                {
                                    //重置计数器
                                    i = 0;
                                    //读取xml
                                    xmlDoc.Load(NextFile.FullName);
                                }
                                else { }
                            }
                            
                            //读取DOMSymbolInstance节点，检查a元件matrix是否为空或为0
                            foreach (XmlElement el in xmlDoc.GetElementsByTagName("DOMSymbolInstance"))
                            {
                                //a元件高低位检测
                                //元件序号定义
                                int anum = int.Parse(NextFile.Name.Substring(1, NextFile.Name.Length - 5));
                                //被引用的首位定义
                                string fname = el.GetAttribute("libraryItemName").Substring(0, 1);
                                //被引用元件序号定义
                                int ianum = int.Parse(el.GetAttribute("libraryItemName").Substring(1, el.GetAttribute("libraryItemName").Length - 1));
                                //检测被引用的是否为a元件
                                if (fname == "a")
                                {
                                    //检测是否引用的元件为高序号
                                    if (anum > ianum) { }
                                    else
                                    {
                                        Console.WriteLine(NextFile.Name.Substring(0, NextFile.Name.Length - 4) + "元件引用高序号" + el.GetAttribute("libraryItemName") + "元件，将会引发错误");
                                    }
                                }
                                else { }


                                //读取matrix节点
                                //新功能更新而停用，用了报错///XmlElement melement = (XmlElement)xmlDoc.SelectSingleNode("DOMSymbolItem/timeline/DOMTimeline/layers/DOMLayer/frames/DOMFrame/elements/DOMSymbolInstance/matrix/Matrix");
                                XmlElement melement = (XmlElement)el.GetElementsByTagName("Matrix")[0];
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
                                if (ma == null || md == null || ma == "0" || md == "0" || ma == "" || md == "")
                                {
                                    //a不存在
                                    if (ma == null || ma == "0" || ma == "")
                                    {
                                        //设a为1.000000
                                        melement.SetAttribute("a", "1.000000");
                                    }
                                    //d不存在
                                    if (md == null || md == "0" || md == "")
                                    {
                                        //设d为1.000000
                                        melement.SetAttribute("d", "1.000000");
                                    }
                                    else { }
                                    //保存xml
                                    xmlDoc.Save(NextFile.FullName);
                                }
                                else { }
                                //重新获得a属性字符串
                                ma = melement.GetAttribute("a");
                                //重新获得d属性字符串
                                md = melement.GetAttribute("d");
                                //保存xml
                                xmlDoc.Save(NextFile.FullName);
                            }
                        }
                        //保存xml
                        xmlDoc.Save(NextFile.FullName);
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
