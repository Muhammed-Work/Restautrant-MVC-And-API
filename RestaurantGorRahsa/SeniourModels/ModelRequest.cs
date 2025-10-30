namespace RestaurantGorRahsa.SeniourModels
{
    public class ModelRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string CustomerID { get; set; }

        public ModelCustomer Customer { get; set; }


    }

}
