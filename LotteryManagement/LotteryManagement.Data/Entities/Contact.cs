using LotteryManagement.Data.Enums;
using LotteryManagement.Infrastructure.SharedKernel;

namespace LotteryManagement.Data.Entities
{
    public class Contact : DomainEntity<string>
    {
        public Contact() { }

        public Contact(string id, string name, string phone, string email,
            string website, string address, string other, double? longtitude, double? latitude, Status status)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            Website = website;
            Address = address;
            Other = other;
            Lng = longtitude;
            Lat = latitude;
            Status = status;
        }

        public string Name { set; get; }


        public string Phone { set; get; }


        public string Email { set; get; }


        public string Website { set; get; }

        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public Status Status { set; get; }
    }
}
