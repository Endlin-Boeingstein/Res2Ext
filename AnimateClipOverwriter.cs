using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//建立a元件引用重写类
class AnimateClipOverwriter
{
    //建立AnimMapper的JSON对象
    public JObject AnimMapper = new JObject();
    //生成a元件引用重写部分
    public void AnimateClipOverwrite(string EPath,JObject animMapper)
    {
        try
        {
            //判定extra.json是否存在，若存在，则执行重写操作
            if (!File.Exists(EPath))
            {
                Console.WriteLine("未检测到xfl文件夹内存在旧版extra.json，将执行默认操作...");
            }
            else
            {
                //读取文本
                string json = File.ReadAllText(EPath);
                //询问是否参考旧版extra.json重写a元件引用信息
                Console.WriteLine("a元件引用部分是否参考旧版extra.json?否则输入n或0并按回车键（不输入按回车默认执行参考操作）");
                //选项收录
                string s = Console.ReadLine();
                //判定是否直接回车，输入它值或直接回车则执行
                if (s == ""|| s == "/n/n"|| s != "0" || s != "n")
                {
                    //将读取文本转换为JSON对象
                    JObject rss = JObject.Parse(json);
                    //将旧a元件引用信息 Object化
                    JObject OldAnimMapper = (JObject)rss["animMapper"];
                    //获取旧a元件引用信息数量
                    int oc = OldAnimMapper.Count;
                    //获取新a元件引用信息数量
                    int ac = animMapper.Count;
                    //判断以避免旧a元件引用信息数量大于新a元件引用信息数量
                    if (oc > ac)
                    {
                        //循环以删除旧a元件引用信息比新a元件引用信息多余部分
                        for (int i = animMapper.Count; OldAnimMapper.Count > animMapper.Count; i++)
                        {
                            //获取最大索引
                            string index = "a" + i.ToString();
                            //判断索引指向的旧a元件引用信息是否存在以移除多余引用信息
                            if (OldAnimMapper.Property(index) == null) { }
                            else
                            {
                                OldAnimMapper.Property(index).Remove();
                            }
                        }
                    }
                    else { }
                    //合并重写a元件引用信息
                    animMapper.Merge(OldAnimMapper, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                    //为上传类赋值
                    AnimMapper = animMapper;
                }
                else { }
            }
        }
        catch
        {
            Console.WriteLine("AnimateClipOverwrite ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }
}

