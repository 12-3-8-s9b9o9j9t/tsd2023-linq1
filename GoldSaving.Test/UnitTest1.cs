using GoldSaving.Lib;
using GoldSaving.Lib.Model;

namespace GoldSaving.Test
{
    public class Tests
    {
        GoldClient goldClient;

        [SetUp]
        public void Setup()
        {
            goldClient = new GoldClient();
        }

        [Test]
        public void CurrentPriceNotNullTest()
        {
            GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();

            Assert.IsNotNull(currentPrice);
            Assert.IsNotNull(currentPrice.Price);
            Assert.Greater(currentPrice.Price, 0.0);
            Assert.AreEqual(DateTime.Now.ToShortDateString(), currentPrice.Date.ToShortDateString());



        }

        [Test]
        public void GetGoldPricesNotNullTest()
        {
            List<GoldPrice> thisYearPrices = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 03, 17)).GetAwaiter().GetResult();

            Assert.IsNotNull(thisYearPrices);
            Assert.Greater(thisYearPrices.Count, 0);
        }
    }
}
