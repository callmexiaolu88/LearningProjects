using System;
using Newtonsoft.Json;
using NLog;
using TVU.SharedLib.BMDDeckLink;

namespace TVUBMTool
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                BMDDeckLinkProxy.Init();
                var hwName = BMDDeckLinkProxy.GetModelName();
                logger.Info($"BlackMagic Name:{hwName}");
                foreach (var item in Environment.GetEnvironmentVariables())
                {
                    logger.Debug(JsonConvert.SerializeObject(item));
                }
                TVUSDICardManager.Instance.Init();
                logger.Info(JsonConvert.SerializeObject(TVUSDICardManager.Instance, Formatting.Indented));
                Console.WriteLine($"=============={hwName}-Card Number:{TVUSDICardManager.Instance.Count}==============");
                while (true)
                {
                    Console.WriteLine($"Input Card number:(the first number is 1)");
                    var info = Console.ReadLine();
                    if (int.TryParse(info, out int index) && index <= TVUSDICardManager.Instance.Count && index > 0)
                    {
                        var card = TVUSDICardManager.Instance.Cards[index - 1];
                        var model = new CardOperation(card);
                        model.Operate();
                    }
                    else
                    {
                        Console.WriteLine($"Invalid selection:{info}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }


    }
}
