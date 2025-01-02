using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextToSpeechLib;

namespace ConsoleApp_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 請將 credentialsPath 替換為您的服務帳戶金鑰檔案路徑
            string credentialsPath = @"C:\Users\User\source\repos\GoogleTextToSpeechLib\ConsoleApp_Test\bin\Release\pure-ivy-446612-a6-98d03dab4133.json";

            // 初始化 GoogleTextToSpeechLib
            var tts = new GoogleLib(credentialsPath);

            // 設定語言、語音性別、語速
            tts.SetLanguage("zh-TW");
            tts.SetVoice(Google.Cloud.TextToSpeech.V1.SsmlVoiceGender.Unspecified);
            tts.SetSpeakingRate(1.5);

            // 設定要轉換的文字
            //tts.SetText("Hello! This is a demonstration of Google Cloud Text-to-Speech API.");
            tts.SetText("你好!歡迎來到Google Speaker");

            // 選擇輸出方式
            Console.WriteLine("選擇輸出方式: ");
            Console.WriteLine("1. 輸出到檔案");
            Console.WriteLine("2. 輸出為 Base64");
            Console.WriteLine("3. 輸出為記憶體流");
            Console.Write("請輸入選項 (1/2/3): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // 輸出到檔案
                    string filePath = "C:\\output.wav";
                    tts.SynthesizeToFile(filePath, "wav");
                    Console.WriteLine($"語音已成功儲存到檔案: {filePath}");
                    break;

                case "2":
                    // 輸出為 Base64 字串
                    string base64String = tts.SynthesizeToBase64();
                    Console.WriteLine("Base64 字串: ");
                    Console.WriteLine(base64String);
                    break;

                case "3":
                    // 輸出為記憶體流
                    using (var stream = tts.SynthesizeToStream())
                    {
                        Console.WriteLine("語音記憶體流已成功產生，可以用於其他處理！");
                    }
                    break;

                default:
                    Console.WriteLine("無效選項！");
                    break;
            }
        }
    }
}
