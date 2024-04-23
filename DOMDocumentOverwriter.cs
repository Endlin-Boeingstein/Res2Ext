using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

//建立DOMDocument重写类
class DOMDocumentOverwriter
{
    //生成DOMDocument重写部分
    public void DOMDocumentOverwrite(string DPath)
    {
        try
        {
            //创建路径文件夹实例
            DirectoryInfo TheFolder = new DirectoryInfo(DPath);
            //获取DOMDocument节点__ABOUT__属性
            //创建xml读取对象
            XmlDocument xmlDoc = new XmlDocument();
            //读取xml
            xmlDoc.Load(TheFolder.FullName);
            //读取DOMDocument节点
            XmlElement element = (XmlElement)xmlDoc.GetElementsByTagName("DOMDocument")[0];
            //设定nag
            string nag = null;
            //读取DOMDocument节点__ABOUT__属性字符串
            nag = element.GetAttribute("__ABOUT__");
            if (nag == null || nag == "0" || nag == "")
            {
                //恢复xml中太极标签
                element.SetAttribute("__ABOUT__", "this XFL is convert from Popcap-AniMation file , by TaiJi .");
                //保存xml
                xmlDoc.Save(TheFolder.FullName);
            }
            else { }

            //宝开化判定
            string filetypeGUID = null;
            string fileGUID = null;
            filetypeGUID = element.GetAttribute("filetypeGUID");
            fileGUID = element.GetAttribute("fileGUID");
            if ((filetypeGUID != null && filetypeGUID != "") || (fileGUID != null && fileGUID != ""))
            {
                //设定main元件工程背景颜色，网格颜色，网格宽，网格高，网格是否可视，标尺是否可视，宽以及高
                string gridColor = null;
                string gridSpacingX = null;
                string gridSpacingY = null;
                string gridVisible = null;
                string rulerVisible = null;
                string backgroundColor = null;
                string width = null;
                string height = null;
                //读取DOMDocument节点背景颜色、网格颜色、网格宽、网格高、网格是否可视、标尺是否可视、宽、高属性字符串
                backgroundColor = element.GetAttribute("backgroundColor");
                gridColor = element.GetAttribute("gridColor");
                gridSpacingX = element.GetAttribute("gridSpacingX");
                gridSpacingY = element.GetAttribute("gridSpacingY");
                gridVisible = element.GetAttribute("gridVisible");
                rulerVisible = element.GetAttribute("rulerVisible");
                width = element.GetAttribute("width");
                height = element.GetAttribute("height");
                //设置网格颜色
                if (gridColor == null || gridColor == "0" || gridColor == "#999999" || gridColor == "")
                {
                    //设置网格颜色为白色
                    element.SetAttribute("gridColor", "#FFFFFF");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置背景颜色
                if (backgroundColor == null || backgroundColor == "0" || backgroundColor == "#FFFFFF" || backgroundColor == "")
                {
                    //设置背景颜色为灰色
                    element.SetAttribute("backgroundColor", "#999999");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置网格宽
                if (gridSpacingX == null || gridSpacingX == "0" || gridSpacingX == "1" || gridSpacingX == "")
                {
                    //设置宽为128
                    element.SetAttribute("gridSpacingX", "128");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置网格高
                if (gridSpacingY == null || gridSpacingY == "0" || gridSpacingY == "1" || gridSpacingY == "")
                {
                    //设置高为128
                    element.SetAttribute("gridSpacingY", "128");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置网格可视
                if (gridVisible == null || gridVisible == "0" || gridVisible == "false" || gridVisible == "")
                {
                    //设置为true
                    element.SetAttribute("gridVisible", "true");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置标尺可视
                if (rulerVisible == null || rulerVisible == "0" || rulerVisible == "false" || rulerVisible == "")
                {
                    //设置为true
                    element.SetAttribute("rulerVisible", "true");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置宽
                if (width == null || width == "0" || width == "1" || width == "")
                {
                    //设置宽为384
                    element.SetAttribute("width", "384");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
                //设置高
                if (height == null || height == "0" || height == "1" || height == "")
                {
                    //设置高为384
                    element.SetAttribute("height", "384");
                    //保存xml
                    xmlDoc.Save(TheFolder.FullName);
                }
                else { }
            }
            else { }
        }
        catch
        {
            Console.WriteLine("DOMDocumentOverwrite ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }
}