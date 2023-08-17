using System;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using TVU.SharedLib.BMDDeckLink;
using TVU.SharedLib.BMDTool;

namespace TVUBMTool
{
    class CardOperation
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        readonly TVUSDICard card;
        public CardOperation(TVUSDICard card)
        {
            this.card = card;
        }

        public void Operate()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"==============Card Index:{card.Index}-Name:{card.DisplayName}-Model:{card.ModelName}==============");
                    Console.WriteLine("Select operate:");
                    Console.WriteLine("1:View Card");
                    Console.WriteLine("2:Set Card");
                    Console.WriteLine("others:back");
                    switch (Console.ReadLine().Trim())
                    {
                        case "1":
                            View();
                            break;
                        case "2":
                            Set();
                            break;
                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        void View()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("==============View==============");
                    Console.WriteLine("Select view:");
                    Console.WriteLine("1:BusyStatus");
                    Console.WriteLine("2:DuplexStatus");
                    Console.WriteLine("3:VideoOutputSquareDivisionSplit");
                    Console.WriteLine("4:DeviceLinkConfiguration");
                    Console.WriteLine("5:DeviceProfiles");
                    Console.WriteLine("6:DeviceActivedProfiles");
                    Console.WriteLine("others:back");
                    switch (Console.ReadLine().Trim())
                    {
                        case "1":
                            Console.WriteLine($"==============BusyStatus=============={Environment.NewLine}{card.CheckBusyStatus()}");
                            break;
                        case "2":
                            Console.WriteLine($"==============DuplexStatus=============={Environment.NewLine}{card.CheckDuplexStatus()}");
                            break;
                        case "3":
                            Console.WriteLine($"==============VideoOutputSquareDivisionSplit=============={Environment.NewLine}{JsonConvert.SerializeObject(card.GetVideoOutputSquareDivisionSplit(), Formatting.Indented)}");
                            break;
                        case "4":
                            Console.WriteLine($"==============DeviceLinkConfiguration=============={Environment.NewLine}{JsonConvert.SerializeObject(card.GetDeviceLinkConfiguration(), Formatting.Indented)}");
                            break;
                        case "5":
                            Console.WriteLine($"==============DeviceProfiles=============={Environment.NewLine}{JsonConvert.SerializeObject(card.GetDeviceProfiles(), Formatting.Indented)}");
                            break;
                        case "6":
                            Console.WriteLine($"==============DeviceActivedProfiles=============={Environment.NewLine}{JsonConvert.SerializeObject(card.GetDeviceActivedProfiles(), Formatting.Indented)}");
                            break;
                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        void Set()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("==============Set==============");
                    Console.WriteLine("Select set:");
                    Console.WriteLine("1:SetDeviceLinkConfiguration");
                    Console.WriteLine("2:SetDeviceProfile");
                    Console.WriteLine("3:SetVideoOutputSquareDivisionSplit");
                    Console.WriteLine("others:back");
                    switch (Console.ReadLine().Trim())
                    {
                        case "1":
                            SetDeviceLinkConfiguration();
                            break;
                        case "2":
                            SetDeviceProfile();
                            break;
                        case "3":
                            SetVideoOutputSquareDivisionSplit();
                            break;
                        default:
                            return;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }


        void SetDeviceLinkConfiguration()
        {
            try
            {
                Console.WriteLine("==============SetDeviceLinkConfiguration==============");
                Console.WriteLine($"Current DeviceLinkConfiguration:{Environment.NewLine}{card.GetDeviceLinkConfiguration()}");
                Console.WriteLine("Select configuration:");
                Console.WriteLine("1:SingleLink");
                Console.WriteLine("2:DualLink");
                Console.WriteLine("3:QuadLink");
                Console.WriteLine("others:back");
                var flag = false;
                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        flag= card.SetDeviceLinkConfiguration(EnumTVUBMLinkConfiguration.SingleLink);
                        break;
                    case "2":
                        flag = card.SetDeviceLinkConfiguration(EnumTVUBMLinkConfiguration.DualLink);
                        break;
                    case "3":
                        flag = card.SetDeviceLinkConfiguration(EnumTVUBMLinkConfiguration.QuadLink);
                        break;
                    default:
                        return;
                }
                if (flag)
                {
                    Console.WriteLine("Set Successfully");
                    Console.WriteLine($"Current DeviceLinkConfiguration:{Environment.NewLine}{card.GetDeviceLinkConfiguration()}");
                }
                else
                {
                    Console.WriteLine("Failed to set");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        void SetDeviceProfile()
        {
            try
            {
                Console.WriteLine("==============SetDeviceProfile==============");
                var profiles = card.GetDeviceProfiles();
                Console.WriteLine($"Current DeviceProfiles:{Environment.NewLine}{JsonConvert.SerializeObject(profiles, Formatting.Indented)}");
                Console.WriteLine($"Current DeviceActivedProfiles:{Environment.NewLine}{JsonConvert.SerializeObject(card.GetDeviceActivedProfiles(), Formatting.Indented)}");
                Console.WriteLine("Select profile number:");
                Console.WriteLine("other:back");
                if (int.TryParse(Console.ReadLine().Trim(), out int index) && profiles.Any(i => i.profileID == index))
                {
                    if (card.SetDeviceProfile(index))
                    {
                        Console.WriteLine("Set Successfully");
                        Console.WriteLine($"Current DeviceActivedProfiles:{Environment.NewLine}{JsonConvert.SerializeObject(card.GetDeviceActivedProfiles(), Formatting.Indented)}");
                    }
                    else
                        Console.WriteLine("Failed to set");
                }
                else
                {
                    Console.WriteLine($"Invalid selection.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        void SetVideoOutputSquareDivisionSplit()
        {
            try
            {
                Console.WriteLine("==============SetVideoOutputSquareDivisionSplit==============");
                Console.WriteLine($"Current VideoOutputSquareDivisionSplit:{Environment.NewLine}{JsonConvert.SerializeObject(card.GetVideoOutputSquareDivisionSplit(), Formatting.Indented)}");
                Console.WriteLine("Select options:");
                Console.WriteLine("1:true");
                Console.WriteLine("2:false");
                Console.WriteLine("others:back");
                var flag = false;
                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        flag = card.SetVideoOutputSquareDivisionSplit(true);
                        break;
                    case "2":
                        flag = card.SetVideoOutputSquareDivisionSplit(false);
                        break;
                    default:
                        return;
                }
                if (flag)
                {
                    Console.WriteLine("Set Successfully");
                    Console.WriteLine($"Current VideoOutputSquareDivisionSplit:{Environment.NewLine}{JsonConvert.SerializeObject(card.GetVideoOutputSquareDivisionSplit(), Formatting.Indented)}");
                }
                else
                {
                    Console.WriteLine("Failed to set");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}
