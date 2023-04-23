namespace Graduate_Work.Models
{
    public class GaneralInfo
    {
        public Package Package { get; set; }
        public PackageType PackageType { get; set; }
        public Department SenderDepartment { get; set; }
        public Department ReciverDepartment { get; set; }
        public Customer Sender { get; set; }
        public Customer Reciver { get; set; }
    }
}
