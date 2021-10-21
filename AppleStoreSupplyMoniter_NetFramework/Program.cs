using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppleStoreSupplyMoniter
{
    public class Program
    {
        static void Main(string[] args)
        {
            SupplyMoniter moniter = new SupplyMoniter();
            Task.Run(async () => await moniter.Monite()).Wait();
        }
    }

    public class SupplyMoniter
    {
        public List<string> Z0YQ_option = new List<string>()
        {
            //运动型表带
            "MKMJ3CH/A,MKUY3FE/A", //午夜色铝金属表壳；英伦薰衣草色运动型表带
            "MKMJ3CH/A,MKUX3FE/A", //午夜色铝金属表壳；金盏花色运动型表带
            "MKMJ3CH/A,MKV13FE/A", //午夜色铝金属表壳；绛樱桃色运动型表带
            "MKMJ3CH/A,MKUN3FE/A", //午夜色铝金属表壳；苜蓿草色运动型表带
            "MKMJ3CH/A,MKUW3FE/A", //午夜色铝金属表壳；深邃蓝色运动型表带
            "MKJP3CH/A",           //午夜色铝金属表壳；午夜色运动型表带_标准号
            "MKMJ3CH/A,MLYT3FE/A", //午夜色铝金属表壳；午夜色运动型表带_大号
            "MKMJ3CH/A,MKUU3FE/A", //午夜色铝金属表壳；星光色运动型表带
            "MKMJ3CH/A,MKUV3CH/A", //午夜色铝金属表壳；红色运动型表带

            //回环式运动表带
            "MKMJ3CH/A,ML303FE/A", //午夜色铝金属表壳；柚粉配小麦色回环式运动表带
            "MKMJ3CH/A,ML323FE/A", //午夜色铝金属表壳；绛樱桃色配松林绿色回环式运动表带
            "MKMJ3CH/A,ML313FE/A", //午夜色铝金属表壳；深邃蓝配苔绿色回环式运动表带
            "MKMJ3CH/A,ML333FE/A", //午夜色铝金属表壳；风暴黑配灰色回环式运动表带
            "MKMJ3CH/A,MLYR3FE/A", //午夜色铝金属表壳；风暴黑配灰色回环式运动表带
            "MKMJ3CH/A,ML8G3CH/A", //午夜色铝金属表壳；红色回环式运动表带

            //Nike 运动表带
            "MKMU3CH/A,ML8A3FE/A", //午夜色铝金属表壳；橙粉配淡粉色 Nike 运动表带
            "MKMU3CH/A,ML8D3FE/A", // 午夜色铝金属表壳；橄榄灰配军裤卡其色 Nike 运动表带
            "MKMU3CH/A,ML8C3FE/A", // 午夜色铝金属表壳；深海军蓝配浅海军蓝色 Nike 运动表带
            "MKL53CH/A", //午夜色铝金属表壳；煤黑配黑色 Nike 运动表带

            //Nike 回环式运动表带
            "MKMU3CH/A,ML363FE/A", // 军裤卡其色
            "MKMU3CH/A,ML343FE/A", // 黑色
            "MKMU3CH/A,ML373FE/A", // 雪峰白色
            "MKMU3CH/A,MJWP3FE/A", // 彩虹版

            //45 毫米银色不锈钢表壳
            "MKJW3CH/A",                //银色不锈钢表壳; 45 毫米银色不锈钢表壳；银色米兰尼斯表带
            "MKMQ3CH/A,ML5T3FE/A",      //银色不锈钢表壳; 黍米色
            "MKMQ3CH/A,ML6D3FE/A",      //银色不锈钢表壳; 英伦薰衣草色
            "MKMQ3CH/A,ML633FE/A",      //银色不锈钢表壳; 绛樱桃色
            "MKMQ3CH/A,ML6N3FE/A",      //银色不锈钢表壳; 深邃蓝色
            "MKMQ3CH/A,ML6Y3CH/A",      //银色不锈钢表壳; 红色
            "MKMQ3CH/A,MJX93FE/A",      //银色不锈钢表壳; 彩虹版 4
            "MKMQ3CH/A,MJXA3FE/A",      //银色不锈钢表壳; 彩虹版 5
            "MKMQ3CH/A,MJXC3FE/A",      //银色不锈钢表壳; 彩虹版 6
            "MKMQ3CH/A,MJXD3FE/A",      //银色不锈钢表壳; 彩虹版 7
            "MKMQ3CH/A,MJXE3FE/A",      //银色不锈钢表壳; 彩虹版 8
            "MKMQ3CH/A,MJXF3FE/A",      //银色不锈钢表壳; 彩虹版 9
        };

        public TimeSpan WholeListQueryInterval { get; set; } = TimeSpan.FromSeconds(60);
        public TimeSpan PerProductQueryInterval { get; set; } = TimeSpan.FromSeconds(0.5);
        public int RepeatCount { get; set; } = 60;
        public bool RepeatInfinite { get; set; } = false;
        public async Task Monite()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"C:\Users\cjw11\Music\宋祖英 - 好日子.wav"; //wav format

            Random rand = new Random();
            int tried = 0;
            StringBuilder sbFound = new StringBuilder();
            while (RepeatInfinite || RepeatCount-- > 0)
            {
                ParallelOptions op = new ParallelOptions();
                op.MaxDegreeOfParallelism = 2;


                // Can be Parallel, but high frequency will result http 503
                foreach (var item in Z0YQ_option)
                {
                    var product = "Z0YQ";
                    var watchCase = "";
                    var watchBand = "";

                    var option = item;

                    var tempArry = item.Split(',');
                    if (tempArry.Length <= 1)
                    {
                        product = item;
                        option = "";
                    }
                    else
                    {
                        watchCase = tempArry[0];
                        watchBand = tempArry[1];
                    }

                    var productTitle = item;

                    try
                    {
                        // store=R484 is store id
                        var url = $"https://www.apple.com.cn/shop/fulfillment-messages?parts.0={product}&option.0={option}&mt=regular&store=R484";
                        var model = await _sendRequest(url);
                        var stores = model?.Body?.Content?.PickupMessage?.Stores;
                        if (stores?.Length >= 1)
                        {
                            var availabileProduct = stores[0].PartsAvailability?.Z0YQ;
                            if (availabileProduct == null)
                            {
                                availabileProduct = stores[0].PartsAvailability?.MKJP3CH_A;
                            }
                            if (availabileProduct == null)
                            {
                                availabileProduct = stores[0].PartsAvailability?.MKL53CH_A;
                            }
                            if (availabileProduct == null)
                            {
                                availabileProduct = stores[0].PartsAvailability?.MKJW3CH_A;
                            }
                            var pickupMessage = $"{availabileProduct?.StorePickupQuote}_{availabileProduct?.PickupSearchQuote}";

                            var productLink = $"https://www.apple.com.cn/shop/buy-watch/apple-watch?option.watch_cases={watchCase}&option.watch_bands={watchBand}&preSelect=false&product={product}&step=detail#";


                            if (pickupMessage.Contains("不") || pickupMessage.Contains("无"))
                            {
                                Log($"{productTitle}\t{pickupMessage}");
                            }
                            else
                            {
                                System.Diagnostics.Process.Start(productLink);
                                player.Play();
                                Log($"************************************************");
                                Log($"{productTitle} 可能有货！\t{pickupMessage}");
                                Log($"************************************************");

                                sbFound.AppendLine($"[{DateTime.Now}] {productTitle}");
                            }
                        }
                        else
                        {
                            Log($"{productTitle}\tNo Store infomation");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    await Task.Delay(PerProductQueryInterval);
                }

                await Task.Delay(WholeListQueryInterval);
                Console.Clear();

                Log($"Tried count:{++tried}");
                Log(sbFound.ToString());
                Log("");
            }
        }

        private HttpClient _client = new HttpClient();
        private async Task<StoreInfoModel> _sendRequest(string url)
        {
            StoreInfoModel model = null;
            string responseStr = string.Empty;
            try
            {
                var response = await _client.GetAsync(url);
                responseStr = await response.Content.ReadAsStringAsync();
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<StoreInfoModel>(responseStr);
            }
            catch (Exception e)
            {
            }
            return model;
        }

        public void Log(string msg)
        {
            Console.WriteLine($"[{DateTime.Now}] {msg}");
        }
    }
}
