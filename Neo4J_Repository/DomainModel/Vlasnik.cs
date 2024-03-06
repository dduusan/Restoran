using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
  public  class Vlasnik
    {
        public String id { get; set; }
        public String ime { get; set; }
        public String prezime { get; set; }
        public String email { get; set; }
        public String sifra { get; set; }

        public List <Restoran> restorani { get; set; }
    }
}
