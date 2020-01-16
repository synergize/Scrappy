using MoversAndShakersScrapingService.Data_Models;
using System.Collections.Generic;
using static MoversAndShakersScrapingService.Data_Models.MoverCardDataModel;

namespace MoversAndShakersScrapingService.Helpers
{
    public class MoverCardDataEqualityComparer : IEqualityComparer<CardInfo>
    {
        public bool Equals(CardInfo x, CardInfo y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else if (x.PriceChange == y.PriceChange && x.Name == y.Name && x.TotalPrice == y.TotalPrice && x.ChangePercentage == y.ChangePercentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(CardInfo obj)
        {
            int hCode = obj.ChangePercentage.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.PriceChange.GetHashCode() ^ obj.TotalPrice.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
