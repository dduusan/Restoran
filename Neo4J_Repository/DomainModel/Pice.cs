using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
public class Pice
    {

        public String ime { get; set; }
        public int idPica { get; set; }
        public Pice(String v)
        {
            this.ime = v;
        }
        public Pice()
        { }
    }

}
