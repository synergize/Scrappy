namespace MoversAndShakersScrapingService.Data_Models
{
    public class MoverCardDataModel
    {
        private string _priceChange;
        private string _name;
        private string _totalPrice;
        private string _changePercentage;

        public string PriceChange
        {
            get { return _priceChange; }
            set { _priceChange = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public string ChangePercentage
        {
            get { return _changePercentage; }
            set { _changePercentage = value; }
        }

        public MoverCardDataModel(string priceChange, string name, string totalPrice, string changePercentage)
        {
            _priceChange = priceChange;
            _name = name;
            _totalPrice = totalPrice;
            _changePercentage = changePercentage;
        }

        public MoverCardDataModel()
        {

        }
    }
}
