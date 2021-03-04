using System.Collections.Generic;

namespace UDM.Insurance.Interface.Data
{
    class GlobalConstants
    {
        public static class BatchCodes
        {
            public static readonly IEnumerable<string> RedeemGift = new[] { "_R", "_NR", "_RTEST", "_NRTEST" };
            public static readonly IEnumerable<string> GiftRedeemed = new[] { "_R", "_RTEST" };
            public static readonly IEnumerable<string> GiftNotRedeemed = new[] { "_NR", "_NRTEST" };
        }
    }
}
