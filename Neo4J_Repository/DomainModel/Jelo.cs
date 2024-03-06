using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
  public  class Jelo
    {

     
        public String ime { get; set; }
        public int jeloId { get; set; }
        public Jelo(String v)
        {
            this.ime = v;
        }
        public Jelo()
        { }
        
    }
}
