using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//建立加密元件创制类
class EncryptClipCreator
{
    //创建位图元件创建对象
    public ImageClipCreator ic = new ImageClipCreator();
    //创建动画元件创建对象
    public AnimateClipCreator ac = new AnimateClipCreator();
    //加密位图名
    public string encrypt_sprite;
    //生成加密元件创建部分
    public void EncryptClipCreate(string Fpath,string Dpath)
    {
        try
        {
            Console.WriteLine("输入的Resources片段含有用以加密的位图信息吗？是则输入1或y，否则输入0或n，输入完成后按回车以继续（不输入直接回车默认不存在）");
            //选项收录
            string s = Console.ReadLine();
            if (s == "" || s == "/n/n" || s == "0" || s == "n") { }
            else
            {
                Console.WriteLine("请输入加密的位图名称并按回车键（输入的内容不是id，而是不带.png后缀的位图名称）");
                encrypt_sprite = Console.ReadLine();
                ic.ImageClipCreate(Fpath, encrypt_sprite,Dpath);
                ac.AnimateClipCreate(Fpath, ic.icname, Dpath);
            }
        }
        catch
        {
            Console.WriteLine("EncryptClipCreate ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }

}