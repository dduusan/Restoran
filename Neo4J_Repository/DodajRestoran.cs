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
    public partial class DodajRestoran : Form
    {
        public GraphClient client;
        public Vlasnik globalniVlasnik;
        public Restoran res;
        public Pice pic;
        public Jelo jel;
        public DodajRestoran()
        {
            InitializeComponent();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Restoran res = new Restoran();
            res.naziv = txtNaziv.Text;
            res.lokacija = txtMesto.Text;
            res.brmesta = txtBrMesta.Text;
            res.id = "22";





            Pice cepi;

            List<Pice> svaPica = new List<Pice>();


            Jelo has;
            List<Jelo> svaJela = new List<Jelo>();
            foreach (String s in checkedListBox1.CheckedItems)
            {
                cepi = new Pice(s);
                cepi.idPica = 22;
                svaPica.Add(cepi);
            }
            foreach (String s in checkedListBox2.CheckedItems)
            {
                has = new Jelo(s);
                has.jeloId = 22;
                svaJela.Add(has);
            }
            foreach (Pice p in svaPica)
            {
                MessageBox.Show(p.ime);
            }

            foreach (Jelo p in svaJela)
            {
                MessageBox.Show(p.ime);
            }


            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("id", res.id);
            queryDict.Add("naziv", res.naziv);
            queryDict.Add("lokacija", res.lokacija);
            queryDict.Add("brmesta", res.brmesta);


            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Restoran {id:'" + res.id + "', naziv:'" + res.naziv
                                                            + "', lokacija:'" + res.lokacija + "', brmesta:'" + res.brmesta
                                                            + "'}) return n",
                                                            queryDict, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
            //List<Restoran> restorani = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query).ToList();

            Dictionary<string, object> queryDict0 = new Dictionary<string, object>();
            var query0 = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Restoran) where r.naziv='" + res.naziv + "'" + "return ID(r)"
                , queryDict0, CypherResultMode.Set);

            int idbroj = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query0).FirstOrDefault();


            Dictionary<string, object> queryDict2 = new Dictionary<string, object>();
            var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Vlasnik), (b: Restoran) WHERE a.email='" + globalniVlasnik.email
                 + "' " + "and ID(b)=" + idbroj + " CREATE(a) -[r: je_vlasnik]->(b) RETURN type(r)",
            queryDict2, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query2);



            foreach (Pice p in svaPica)
            {
                Dictionary<string, object> queryDictP = new Dictionary<string, object>();
                var queryP = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Pice {ime:'" + p.ime + "'" + ", idPica:" + p.idPica + "}) return n",
                                                           queryDictP, CypherResultMode.Set);
                ((IRawGraphClient)client).ExecuteCypher(queryP);

                Dictionary<string, object> queryDictIzvuci = new Dictionary<string, object>();
                var queryIzvuci = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Pice) where r.idPica=" + p.idPica + " return ID(r)"
                    , queryDictIzvuci, CypherResultMode.Set);
                int idPica = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(queryIzvuci).FirstOrDefault();

                Dictionary<string, object> queryDictKK = new Dictionary<string, object>();
                var queryKK = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Pice) where r.idPica=" + p.idPica + " set r.idPica=ID(r)"
                    , queryDictKK, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(queryKK);



                Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
                var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Restoran), (b:Pice) WHERE ID(a)=" + idbroj
                    + " and ID(b)=" + idPica + " CREATE(a) -[r: sadrzi]->(b) RETURN type(r)",
                queryDict3, CypherResultMode.Set);
                ((IRawGraphClient)client).ExecuteCypher(query3);
            }
            foreach (Jelo j in svaJela)
            {
                Dictionary<string, object> queryDictJ = new Dictionary<string, object>();
                var queryJ = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Jelo {ime:'" + j.ime + "'" + ", jeloId:" + j.jeloId + "}) return n",
                                                           queryDictJ, CypherResultMode.Set);
                ((IRawGraphClient)client).ExecuteCypher(queryJ);


                Dictionary<string, object> queryDictIzvuci = new Dictionary<string, object>();
                var queryIzvuci = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Jelo) where r.jeloId=" + j.jeloId + " return ID(r)"
                    , queryDictIzvuci, CypherResultMode.Set);
                int idJelo = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(queryIzvuci).FirstOrDefault();


                Dictionary<string, object> queryDictKK = new Dictionary<string, object>();
                var queryKK = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Jelo) where r.jeloId=" + j.jeloId + " set r.jeloId=ID(r)"
                    , queryDictKK, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(queryKK);

                Dictionary<string, object> queryDict3 = new Dictionary<string, object>();
                var query3 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Restoran), (b:Jelo) WHERE ID(a)=" + idbroj
                    + " and ID(b)=" + idJelo + " CREATE(a) -[r: sadrzi_jelo]->(b) RETURN type(r)",
                queryDict3, CypherResultMode.Set);
                ((IRawGraphClient)client).ExecuteCypher(query3);
                this.Close();


                //string mailVlasnika = globalniVlasnik.email;
                // var query2 = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Vlasnik), (b: Restoran) WHERE a.email =~ {mailVlasnika} and b.id =~ {res.id} CREATE(a) -[r: je_vlasnik]->(b)RETURN type(r)",
                //  queryDict, CypherResultMode.Set);
            }
           
        }
    }
}
