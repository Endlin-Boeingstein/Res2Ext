using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		//定义文件路径
		string filepath;
		//读取文件并判断是直接拖入程序图标读取还是打开程序拖入窗体读取
		//拖入程序图标读取
		if (args.Length > 0)
		{
			FileInfo fileInfo = new FileInfo(args[0]);
			Console.WriteLine("版本号：1.8.3-public-alpha\t作者：Endlin Boeingstein（滨敔滨纵凝）\n编译时间：2021年12月7日14时32分\t协助调试：小黑_yyds、Min_mozj、ATSGF\n在使用本软件前请提前备份好文件，否则后果自负");
			Console.WriteLine("文件路径：" + fileInfo.FullName);
			filepath = fileInfo.FullName;
		}
		//打开程序拖入窗体读取
		else
		{
			Console.WriteLine("版本号：1.8.3-public-alpha\t作者：Endlin Boeingstein（滨敔滨纵凝）\n编译时间：2021年12月7日14时32分\t协助调试：小黑_yyds、Min_mozj、ATSGF\n在使用本软件前请提前备份好文件，否则后果自负");
			Console.WriteLine("请将文件拖入窗体，并按回车键");
			//新功能更新而停用///Console.WriteLine("如果你的文件在C盘（尤其是桌面），那么请退出程序，直接将文件拖放到本应用的图标即可，拖窗体模式无权限修改C盘文件");
			filepath = Console.ReadLine().Trim('"');
		}
		//创建jjof实例
		JudgeJoF jjof = new JudgeJoF();
		//运行jjof实例的判断语句
		jjof.Judge(filepath);
		//提示Done
		Console.WriteLine("Done");
		//提示按任意键继续
		Console.WriteLine("Press any key to continue...");
		//输入任意键退出
		Console.ReadLine();
	}
}
