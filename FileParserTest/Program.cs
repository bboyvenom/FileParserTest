using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace FileParserTest
{
	class Program
	{
		private static string data;
		private static List<string> resList;
		private static string[] fResData, arrData;
		private static string[] repStr = new string[] { @"\t", @"\n", @"\r", @"." };

		static void Main(string[] args)
		{
			readFile("D:\\i.txt");

			arrData = parseData(data, arrData);
			resList = writeList(arrData);
			fResData = writeFArr(arrData);

			writeFile("D:\\o.txt");
		}

		private static void readFile(string path)
		{
			StreamReader sr = new StreamReader(path);
			data = sr.ReadToEnd().ToLower();
		}

		private static string[] parseData(string data, string[] arrData)
		{
			foreach (string s in repStr)
				data = data.Replace(s, " ").Trim();
			arrData = Regex.Split(data, @"\s+");
			return arrData;
		}

		private static List<string> writeList(string[] arrData)
		{
			resList = new List<string>();
			for (int i = 0; i < arrData.Length; i++)
				resList.Add(arrData[i]);
			return resList;
		}

		private static string[] writeFArr(string[] arrData)
		{
			fResData = arrData.ToArray();
			for (int i = 0; i < arrData.Length; i++)
				fResData[i] = arrData[i].Substring(0, 1);
			fResData = fResData.Distinct().OrderBy(o => o).ToArray();
			return fResData;
		}

		private static void writeFile(string path)
		{
			StreamWriter sw = new StreamWriter(path);

			for (int i = 0; i < fResData.Length; i++)
			{
				sw.WriteLine(fResData[i]);

				var count = resList
					.Where(x => x.StartsWith(fResData[i]))
					.GroupBy(x => x)
					.Select(x => new { Word = x.Key, Count = x.Count() })
					.OrderByDescending(x => x.Count);

				foreach (var x in count)
					sw.WriteLine(x.Word + " - " + x.Count);
			}
			sw.Close();
		}
	}
}
