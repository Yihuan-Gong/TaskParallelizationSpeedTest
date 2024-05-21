using StreamLibarary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dir = "D:\\研究所檔案\\軟體開發學習與就業\\軟體開發家教課\\程式碼\\Sub11_AsyncAwait\\TaskPractice\\testData";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int lineNum = 10000000;
            int batchSize = 100000;
            string fileName = "MOCK_DATA_10M";

            ParallelizedBatch(lineNum, batchSize, dir, fileName).GetAwaiter().GetResult();

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Console.ReadKey();
        }


        static void SerializedBatch(int lineNum, int batchSize, string dir, string fileName)
        {
            int batchNum = lineNum / batchSize;

            for (int i = 0; i < batchNum; i++)
            {
                var data = CsvHelper.ReadCSV<UserModel>($"{dir}\\{fileName}.csv", i * batchSize + 1, batchSize);
                CsvHelper.WriteCSV($"{dir}\\{fileName}\\write_batch_{i}.csv", data, append: false, createDir: true);
                GC.Collect();
            }
        }


        static async Task ParallelizedBatch(int lineNum, int batchSize, string dir, string fileName)
        {
            List<Task> tasks = new List<Task>();

            int batchNum = lineNum / batchSize;

            // var let
            for (int i = 0; i < batchNum; i++)
            {
                int fileIndex = i;
                tasks.Add(Task.Run(() =>
                {
                    var data = CsvHelper.ReadCSV<UserModel>($"{dir}\\{fileName}.csv", fileIndex * batchSize + 1, batchSize);
                    CsvHelper.WriteCSV($"{dir}\\{fileName}_parallel\\write_batch_{fileIndex}.csv", data, append: false, createDir: true);
                    GC.Collect();
                }));
                //Thread.Sleep(1000);
            }

            await Task.WhenAll(tasks);
        }
    }
}
