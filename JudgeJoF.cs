﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//建立判断类
class JudgeJoF
{
	//判断完成后路径定义
	public string Jpath = null;
	public string Fpath = null;
	//创建mdf实例
	public MediaDataFormater mdf = new MediaDataFormater();
	//创建ddo实例
	public DOMDocumentOverwriter ddo = new DOMDocumentOverwriter();
	//创建mco实例
	public MainClipOverwriter mco = new MainClipOverwriter();
	//创建oco实例
	public OtherClipOverwriter oco = new OtherClipOverwriter();
	//创建ce实例
	public ClipEncryptor ce = new ClipEncryptor();
	//判断第二个路径是否为json文件
	public void ReJudgeJ(string filepath)
    {
		try
		{
			if (File.Exists(filepath))
			{
				Console.WriteLine("已检测到为json文件");
				this.Jpath = filepath;
			}
			else if (Directory.Exists(filepath))
			{
				Console.WriteLine("已检测到为xfl文件夹，请再将resources.json的分解片段拖入窗体，并按回车键");
				this.Fpath = filepath;
				ReJudgeJ(Console.ReadLine().Trim('"'));
			}
			else
			{
				Console.WriteLine("未检测到文件或文件夹！请检查！");
			}
		}
		catch
		{
			Console.WriteLine("ERROR");

		}
	}
	//判断第二个路径是否为dir文件夹
	public void ReJudgeF(string filepath)
	{
		try
		{
			if (File.Exists(filepath))
			{
				Console.WriteLine("已检测到为json文件，请再将xfl文件夹拖入窗体，并按回车键");
				this.Jpath = filepath;
				ReJudgeF(Console.ReadLine().Trim('"'));
			}
			else if (Directory.Exists(filepath))
			{
				Console.WriteLine("已检测到为xfl文件夹");
				this.Fpath = filepath;
			}
			else
			{
				Console.WriteLine("未检测到文件或文件夹！请检查！");
			}
		}
		catch
		{
			Console.WriteLine("ERROR");

		}
	}
	//判断是json还是dir路径
	public void Judge(string filepath)
	{

		try
		{
			if (File.Exists(filepath))
			{
				Console.WriteLine("已检测到为json文件，请再将xfl文件夹拖入窗体，并按回车键");
				this.Jpath = filepath;
				ReJudgeF(Console.ReadLine().Trim('"'));
				mdf.MediaDataFormat(this.Jpath, this.Fpath + "\\LIBRARY");
				ddo.DOMDocumentOverwrite(this.Fpath + "\\DOMDocument.xml");
				mco.MainClipOverwrite(this.Fpath + "\\LIBRARY\\main.xml");
				oco.OtherClipOverwrite(this.Fpath + "\\LIBRARY");
				ce.ClipEncrypt(this.Jpath, this.Fpath + "\\LIBRARY");
			}
			else if (Directory.Exists(filepath))
			{
				Console.WriteLine("已检测到为xfl文件夹，请再将resources.json的分解片段拖入窗体，并按回车键");
				this.Fpath = filepath;
				ReJudgeJ(Console.ReadLine().Trim('"'));
				mdf.MediaDataFormat(this.Jpath, this.Fpath + "\\LIBRARY");
				ddo.DOMDocumentOverwrite(this.Fpath + "\\DOMDocument.xml");
				mco.MainClipOverwrite(this.Fpath + "\\LIBRARY\\main.xml");
				oco.OtherClipOverwrite(this.Fpath + "\\LIBRARY");
				ce.ClipEncrypt(this.Jpath, this.Fpath + "\\LIBRARY");
			}
			else
			{
				Console.WriteLine("未检测到文件或文件夹！请检查！");
			}
		}
		catch
		{
			Console.WriteLine("ERROR");

		}

	}
}