using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryManagement.Data.Entities
{
    [Table("OwnerBanks")]
    public class OwnerBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public string FullNameOwner { get; set; }

        public string Branch { get; set; }
        
        public string AccountNumber { get; set; }

        public string BankName { get; set; }
    }
}
