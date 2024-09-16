using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Event
{
    public  class CreatLote
    {
        public CreatLote(Guid id, string name, string description, float startPrice, float endPrice, Guid idAuctione)
        {
            Id=id;
            Name=name;
            Description=description;
            StartPrice=startPrice;
            EndPrice=endPrice;
            IdAuctione=idAuctione;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public float StartPrice { get; set; }
        public float EndPrice { get; set; } = 0;
        public Guid? IdAuction { get; set; } 
    }
}
