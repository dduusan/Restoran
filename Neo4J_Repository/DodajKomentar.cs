using Neo4J_Repository.DomainModel;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo4J_Repository
{
    public partial class DodajKomentar : Form
    {
        public int selektovanRestoran;
        public GraphClient client;
       public Musterija globalnaM;
       public Restoran globalniR;
        public DodajKomentar()
        {
            InitializeComponent();
        }
        public DodajKomentar(Restoran r, Musterija m, int idRes)
        {
            InitializeComponent();
            globalnaM = m;
            globalniR = r;
            selektovanRestoran = idRes;
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string unetaporuka = txtKom.Text;
            int ocena1 = Int32.Parse(txtOcena.Text);

            //globalniR.ocena += ocena; moglo je u bazu

            Komentar novi = new Komentar();
            novi.poruka = unetaporuka;
            novi.ocena = ocena1;
            string maxId = getMaxId();

            try
            {
                int mId = Int32.Parse(maxId);
                novi.komentarId = (++mId).ToString();
            }
            catch (Exception exception)
            {
                novi.komentarId = "";
            }

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var queryK = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Komentar {komentarId:'" +novi.komentarId+ "'" +", poruka:'" + novi.poruka+ "'" + ", ocena:" +novi.ocena + "}) return n",
                                                            queryDict, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(queryK);

            Dictionary<string, object> queryDictIzvuci = new Dictionary<string, object>();
            var queryIzvuci = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Komentar) where r.komentarId='" + novi.komentarId + "'" + " return ID(r)"
                , queryDictIzvuci, CypherResultMode.Set);
            int idKomentara = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(queryIzvuci).FirstOrDefault();
            

            Dictionary<string, object> queryDictKK = new Dictionary<string, object>();
            var queryKK = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Komentar) where r.komentarId='"+ novi.komentarId + "'" +" set r.komentarId=ID(r)"
                , queryDictKK, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(queryKK); //kopira pravi ID u komentarID radi lakseg raspoznavanja kasnije 

            

            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Komentar), (b: Restoran) WHERE ID(a)=" 
              + idKomentara+ " and ID(b)=" + selektovanRestoran + " CREATE(a) -[r: za]->(b) RETURN type(r)",
            queryDict2, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query2);

            Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
            var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Musterija), (b: Komentar) WHERE a.email='" + globalnaM.email
                 + "' " + "and ID(b)=" + idKomentara+  " CREATE(a) -[r: je_komentarisao]->(b) RETURN type(r)",
            queryDict3, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query3);
            this.Close();
            //novi.komentarId = generateID();
            /* novi.musterijaId = globalnaM.id;
             novi.restoranId = globalniR.id;



             queryDict.Add("komentarId", novi.komentarId);
             queryDict.Add("restoranId", novi.restoranId);
             queryDict.Add("musterijaId", novi.musterijaId);
             queryDict.Add("poruka", novi.poruka);

             var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Komentar {komentarId:'" + novi.komentarId + "', musterijaId:'" + novi.musterijaId
                                                             + "', restoranId:'" + novi.restoranId + "', poruka:'" + novi.poruka
                                                             + "'}) return n",
                                                             queryDict, CypherResultMode.Set);
             int unetaocena = Int32.Parse(txtOcena.Text);
             unetaocena += globalniR.ocena;
             string restoranId = globalniR.id;
             var query2 = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Restoran) and has(n.id) and n.id =~ {restoranId} set n.ocena = {unetaocena} return n",
                                                             new Dictionary<string, object>(), CypherResultMode.Set);
             string globalnaMusID = globalnaM.id;
             string komentarId = novi.musterijaId;
             string komentarIdRestoran = novi.restoranId;

             var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Musterija), (b: Komentar) WHERE a.id =~ {globalnaMusID} and b.id =~ {komentarId} CREATE(a) -[r: je_komentarisao]->(b)RETURN type(r)",
                                                           queryDict, CypherResultMode.Set);

             var query4 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Komentar), (b: Restoran) WHERE a.id =~ {komentarIdRestoran} and b.id =~ {restoranId} CREATE(a) -[r: odnosi_se]->(b)RETURN type(r)",
                                                           queryDict, CypherResultMode.Set);
            */
        }
        public static string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }
        public String getMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where exists(n.id) return max(n.id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
