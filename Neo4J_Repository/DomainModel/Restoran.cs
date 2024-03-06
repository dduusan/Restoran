using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
 public   class Restoran
    {
        public String id { get; set; }
        public String idVlasnika { get; set; }
        public String naziv { get; set; }
        public String lokacija { get; set; }
        public String brmesta { get; set; }

        public int ocena { get; set; }
        public List<Jelo> jela { get; set; }
        public List<Pice> pica { get; set; }

        public List<Komentar> komentari { get; set; }
    }
}
