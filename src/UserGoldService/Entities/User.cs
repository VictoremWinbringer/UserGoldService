using System;

namespace UserGoldService.Entities
{
    public class User
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public decimal Gold { get; set; }
    }
}
