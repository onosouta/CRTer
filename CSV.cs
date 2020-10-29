using System;
using System.Collections.Generic;
using System.IO;

namespace Game2
{
    //csvファイルを読み込む
    class CSV
    {
        private List<string[]> list = new List<string[]>();

        //csvファイルを読み込む
        public int[][] Load(string _name)
        {
            list.Clear();//初期化

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(@"Content/" + _name + ".csv");

                while (true)
                {
                    if (sr.EndOfStream)//ストリームの末尾
                    {
                        break;
                    }

                    list.Add(sr.ReadLine().Split(','));//追加
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ファイルが見つかりません");
            }

            if (sr != null)
            {
                sr.Close();//ファイルを閉じる
            }

            return IntArray();
        }

        //csvファイルのデータをint[][]に変換
        private int[][] IntArray()
        {
            string[][] sa = list.ToArray();

            int[][] int_array = new int[sa.Length][];   //行の配列

            for (int i = 0; i < sa.Length; i++)
            {
                int_array[i] = new int[sa[i].Length];   //列の配列

                for (int j = 0; j < sa[i].Length; j++)
                {
                    int_array[i][j] = int.Parse(sa[i][j]);//int型にパース
                }
            }

            return int_array;
        }
    }
}
