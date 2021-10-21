using Newtonsoft.Json;

namespace AppleStoreSupplyMoniter
{
    public class StoreInfoModel
    {
        [JsonProperty("head")]
        public Head Head { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }
    }

    public class Head
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Body
    {
        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    public class Content
    {
        [JsonProperty("PickupMessage")]
        public PickupMessage PickupMessage { get; set; }
    }

    public class PickupMessage
    {
        [JsonProperty("stores")]
        public Store[] Stores { get; set; }

        [JsonProperty("pickupLocation")]
        public string PickupLocation { get; set; }
    }

    public class Store
    {
        [JsonProperty("partsAvailability")]
        public PartsAvailability PartsAvailability { get; set; }
    }

    public class PartsAvailability
    {
        [JsonProperty("Z0YQ", NullValueHandling = NullValueHandling.Ignore)]
        public AvailabilityProduct Z0YQ { get; set; }

        [JsonProperty("MKJP3CH/A", NullValueHandling = NullValueHandling.Ignore)]
        public AvailabilityProduct MKJP3CH_A { get; set; }

        [JsonProperty("MKL53CH/A", NullValueHandling = NullValueHandling.Ignore)]
        public AvailabilityProduct MKL53CH_A { get; set; }

        [JsonProperty("MKJW3CH/A", NullValueHandling = NullValueHandling.Ignore)]
        public AvailabilityProduct MKJW3CH_A { get; set; }
    }

    public class AvailabilityProduct
    {
        [JsonProperty("storePickupQuote")]
        public string StorePickupQuote { get; set; }

        [JsonProperty("pickupSearchQuote")]
        public string PickupSearchQuote { get; set; }
    }
}
