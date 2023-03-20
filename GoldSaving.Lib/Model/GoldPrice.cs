using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoldSaving.Lib.Model
{
    public class GoldPrice
    {
        [JsonProperty("Data")]
        public DateTime Date { get; set; }

        [JsonProperty("Cena")]
        public double Price { get; set; }
    }
}
