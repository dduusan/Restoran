using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
 public   class Komentar
    {
        public string komentarId { get; set; }
        public string musterijaId { get; set; }

        public string restoranId { get; set; }
        public string poruka { get; set; }
        public int ocena { get; set; }
    }
}
