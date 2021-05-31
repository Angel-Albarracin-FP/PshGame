using System;

namespace PshGameApi.Models
{
    public class Stat
    {
        public int ID { get; set; }
        public User User { get; set; }
        public int Score { get; set; }
        public DateTime Created { get; set; }
    }
}
