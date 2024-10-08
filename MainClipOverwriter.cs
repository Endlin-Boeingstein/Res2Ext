﻿using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

//建立main元件修复重写类
class MainClipOverwriter
{
    //生成main元件修复重写部分
    public void MainClipOverwrite(string MPath)
    {
        try
        {
            //创建路径文件夹实例
            DirectoryInfo TheFolder = new DirectoryInfo(MPath);
            //创建xml读取对象
            XmlDocument xmlDoc = new XmlDocument();
            Console.WriteLine("修复检测图层帧间空帧中......");
            //创建新xml读取对象
            XmlDocument newxmlDoc = new XmlDocument();
            newxmlDoc.Load(TheFolder.FullName);
            xmlDoc = newxmlDoc;
            //时间长而弃用//重载xml
            //时间长而弃用///xmlDoc.Load(TheFolder.FullName);
            //获取根节点root
            XmlNode root = xmlDoc.DocumentElement;
            //获取节点layers
            //新功能更新而停用///XmlElement layers = (XmlElement)xmlDoc.GetElementsByTagName("layers")[0];
            XmlNode layers = root.FirstChild.FirstChild.FirstChild;
            //获取layers图层列表
            XmlNodeList layersnodeList = layers.ChildNodes;
            
            //检测
            for (int i = 0; i < layersnodeList.Count; i++)
            {
                //转换为Xml节点
                XmlNode node = layersnodeList[i];
                //转换DOMLayer为XmlElement以便于识别是否存在实帧
                XmlElement DOMLayer = (XmlElement)node;
                //判断是否存在DOMSymbolInstance和DOMBitmapInstance，以判定是否为帧间空帧，默认为0(遇到空帧且值为1则存在帧间空帧)
                int dsitrue = 0;
                //判定前空后实数
                int nullandhasele = 0;


                //先行检测
                //记录帧间空帧所在帧数
                for (int j = 0; j < DOMLayer.FirstChild.ChildNodes.Count; j++)
                {
                    //转换为XmlElement
                    XmlElement DOMFrame = (XmlElement)DOMLayer.FirstChild.ChildNodes[j];
                    if (DOMFrame.GetElementsByTagName("DOMBitmapInstance").Count != 0 || DOMFrame.GetElementsByTagName("DOMSymbolInstance").Count != 0)
                    {
                        if (dsitrue == 0)
                        {
                            nullandhasele++;
                        }
                        else { }
                        dsitrue = 1;
                    }
                    else
                    {
                        if (dsitrue == 1)
                        {
                            //获取帧间空帧的图层名称
                            string nname = DOMLayer.GetAttribute("name");
                            //获取帧间空帧的帧位置
                            int nindex = int.Parse(DOMFrame.GetAttribute("index"));
                            //获取帧间空帧的帧长度
                            int nduration;
                            if (DOMFrame.GetAttribute("duration") == null || DOMFrame.GetAttribute("duration") == "")
                            {
                                nduration = 1;
                            }
                            else
                            {
                                nduration = int.Parse(DOMFrame.GetAttribute("duration"));
                            }
                            Console.WriteLine("main元件图层" + nname + "第" + nindex + "帧（在Adobe Animate中为第" + (nindex + 1) + "帧）为帧间空帧，长度" + nduration + "帧，将会引发错误");
                            Console.WriteLine("元件的" + DOMLayer.GetAttribute("name") + "图层的帧间空帧已处理");
                        }
                        else { }
                        dsitrue = 0;
                    }
                }

                //处理
                if (nullandhasele > 1)
                {
                    //重置
                    dsitrue = 0;
                    //重置
                    nullandhasele = 0;
                    //记录图层序号
                    int layernum = 0;
                    //预置addDOMLayer节点，新建图层
                    XmlElement addDOMLayer = xmlDoc.CreateElement("DOMLayer", xmlDoc.DocumentElement.NamespaceURI);
                    //预置addframes节点
                    XmlElement addframes = xmlDoc.CreateElement("frames", xmlDoc.DocumentElement.NamespaceURI);
                    //设name为DOMLayer的name值（此处会出现极大卡顿，暂无法修复）
                    addDOMLayer.SetAttribute("name", DOMLayer.GetAttribute("name") + "_" + layernum);
                    //将addframes作为addDOMLayer的子节点
                    addDOMLayer.AppendChild(addframes);



                    //记录帧间空帧所在帧数
                    for (int j = 0; j < DOMLayer.FirstChild.ChildNodes.Count; j++)
                    {
                        //转换为XmlElement
                        XmlElement DOMFrame = (XmlElement)DOMLayer.FirstChild.ChildNodes[j];




                        if (DOMFrame.GetElementsByTagName("DOMBitmapInstance").Count != 0 || DOMFrame.GetElementsByTagName("DOMSymbolInstance").Count != 0)
                        {
                            if (dsitrue == 0)
                            {
                                nullandhasele++;
                            }
                            else { }
                            dsitrue = 1;
                        }
                        else
                        {
                            //已迁移至检测
                            dsitrue = 0;
                        }
                        //如果未检测到空帧
                        if (nullandhasele <= 1)
                        {
                            //此处填充新建图层的实帧前的空帧
                            //预置addDOMFrame节点
                            XmlElement addDOMFrame = xmlDoc.CreateElement("DOMFrame", xmlDoc.DocumentElement.NamespaceURI);
                            //设index为DOMFrame的index值
                            addDOMFrame.SetAttribute("index", DOMFrame.GetAttribute("index"));
                            //设duration为DOMFrame的duration值
                            string fduration = DOMFrame.GetAttribute("duration");
                            if (fduration == "" || fduration == "0" || fduration == null)
                            {
                                fduration = "1";
                            }
                            else { }
                            addDOMFrame.SetAttribute("duration", fduration);
                            //将addDOMFrame作为addframes的子节点
                            addframes.AppendChild(addDOMFrame);
                        }
                        else
                        {
                            //将本DOMFrame作为addframes的子节点
                            addframes.AppendChild(DOMFrame);
                            //回落
                            j--;
                        }
                    }
                    if (nullandhasele <= 1) { }
                    else
                    {
                        //将addDOMLayer插在DOMLayer的前面
                        root.FirstChild.FirstChild.FirstChild.InsertBefore(addDOMLayer, node);
                        Console.WriteLine("已提取main元件的" + DOMLayer.GetAttribute("name") + "图层的帧间空帧并新建图层");
                        //图层下标增加
                        layernum++;
                        //回落
                        i--;
                        //保存xml
                        xmlDoc.Save(TheFolder.FullName);
                    }
                }
                else { }
            }
            Console.WriteLine("图层帧间空帧修复检测完成");



            Console.WriteLine("开始对main元件进行删除全空和全空帧图层以及删除图层末尾空帧......");

            //读取xml
            ////xmlDoc.Load(TheFolder.FullName);
            ///获取根节点root
            root = xmlDoc.DocumentElement;
            //获取节点layers
            layers = root.FirstChild.FirstChild.FirstChild;
            //获取layers图层列表
            layersnodeList = layers.ChildNodes;
            //预置全空或全空帧图层删除列表
            List<XmlNode> LayersToRemove = new List<XmlNode>();
            //预置删除图层末尾空帧列表
            List<XmlNode> LayersToOverwrite = new List<XmlNode>();
            //预置空帧列表
            List<XmlNode> FramesToRemove = new List<XmlNode>();

            Console.WriteLine("删除全空和全空帧图层中......");
            //删除全空或全空帧图层
            foreach (XmlNode node in layersnodeList)
            {
                //转换DOMLayer为XmlElement以便于识别是否存在实帧
                XmlElement DOMLayer = (XmlElement)node;
                //判断是否存在DOMSymbolInstance，以判定是否为全空或全空帧，默认为0(结果为0则为全空或全空帧)
                int dsiexist = 0;
                foreach (XmlElement element in DOMLayer.ChildNodes)
                {
                    if (element.GetElementsByTagName("DOMBitmapInstance").Count != 0 || element.GetElementsByTagName("DOMSymbolInstance").Count != 0)
                    {
                        dsiexist = 1;
                    }
                    else { }
                }
                //积累待删除记录列表
                if (dsiexist == 0)
                {
                    LayersToRemove.Add(node);
                    //node XmlElement化
                    XmlElement enode = (XmlElement)node;
                    //获取被删图层名称
                    string nenode = enode.GetAttribute("name");
                    Console.WriteLine("删除图层" + nenode);
                    continue;
                }
                else { }
            }
            //根据删除列表删除全空或全空帧图层
            foreach (XmlNode node in LayersToRemove)
            {
                node.ParentNode.RemoveChild(node);
                //保存xml
                xmlDoc.Save(TheFolder.FullName);
            }


            Console.WriteLine("删除图层末尾空帧中......");
            //重载xml
            ////xmlDoc.Load(TheFolder.FullName);
            //获取根节点root
            root = xmlDoc.DocumentElement;
            //获取节点layers
            layers = root.FirstChild.FirstChild.FirstChild;
            //获取layers图层列表
            layersnodeList = layers.ChildNodes;
            //删除末尾空帧
            foreach (XmlNode node in layersnodeList)
            {
                //转换DOMLayer为XmlElement以便于识别是否存在实帧
                XmlElement DOMLayer = (XmlElement)node;
                //判断是否存在DOMSymbolInstance，以判定是否为末尾空帧，默认为0(最终结果为0则存在末尾空帧)
                int dsitrue = 0;
                foreach (XmlElement element in DOMLayer.FirstChild.ChildNodes)
                {
                    if (element.GetElementsByTagName("DOMBitmapInstance").Count != 0 || element.GetElementsByTagName("DOMSymbolInstance").Count != 0)
                    {
                        dsitrue = 1;
                    }
                    else
                    {
                        dsitrue = 0;
                    }
                }
                //积累待重写记录列表
                if (dsitrue == 0)
                {
                    LayersToOverwrite.Add(node);
                    //node XmlElement化
                    XmlElement enode = (XmlElement)node;
                    //获取被删末尾空帧的图层名称
                    string nenode = enode.GetAttribute("name");
                    Console.WriteLine("删除图层" + nenode + "的末尾空帧");
                    continue;
                }
                else { }
            }
            //根据删除列表重写末尾空帧图层
            foreach (XmlNode node in LayersToOverwrite)
            {
                //判断是否存在DOMSymbolInstance，以判定是否为末尾空帧，默认为0(最终结果为0则存在末尾空帧)
                int dsitrue = 0;
                //倒序检测空帧
                for (int i= node.FirstChild.ChildNodes.Count-1; dsitrue == 0; i--)
                {
                    //取需要删除的空帧
                    XmlNode element = node.FirstChild.ChildNodes[i];
                    //转换DOMFrame为XmlElement以便于识别是否存在实帧
                    XmlElement DOMFrame = (XmlElement)element;
                    if (DOMFrame.GetElementsByTagName("DOMBitmapInstance").Count != 0 || DOMFrame.GetElementsByTagName("DOMSymbolInstance").Count != 0)
                    {
                        dsitrue = 1;
                    }
                    else
                    {
                        FramesToRemove.Add(element);
                        continue;
                    }
                }
            }
            //根据删除列表删除空帧
            foreach (XmlNode element in FramesToRemove)
            {
                element.ParentNode.RemoveChild(element);
                //保存xml
                xmlDoc.Save(TheFolder.FullName);
            }
            Console.WriteLine("图层修复检测完成");
        }
        catch
        {
            Console.WriteLine("MainClipOverwrite ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }
}
