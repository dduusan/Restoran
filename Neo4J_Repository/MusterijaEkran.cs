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
    public partial class MusterijaEkran : Form
    {
        public Vlasnik globalniVlasnik;
        public Musterija globalnaMusterija; // da l treba
        public GraphClient client;
        public List<Restoran> listaRestorana; // pitanje?????

        public MusterijaEkran()
        {
            InitializeComponent();
        }
        public MusterijaEkran(Musterija m)
        {
            InitializeComponent();
            globalnaMusterija = m;
            this.popuniInicijalno();
         
        }
        public void popuniInicijalno()
        {

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Id Restorana", 100);
            listView1.Columns.Add("Naziv", 100);
            listView1.Columns.Add("Lokacija", 100);
            listView1.Columns.Add("Broj Mesta", 100);

        }

        public void add(string idR, string naziv, string lokacija, string brojmesta)
        {
            String[] row = { idR, naziv, lokacija, brojmesta};
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                String idRestoran = this.listView1.SelectedItems[0].SubItems[0].Text;

                int praviId = Int32.Parse(idRestoran);

                var query = new Neo4jClient.Cypher.CypherQuery("match (n:Restoran) where ID(n) =" + praviId + " return n",
                                                         new Dictionary<string, object>(), CypherResultMode.Set);
       
                Restoran trazeni = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query).FirstOrDefault();

                Dictionary<string, object> queryDictP = new Dictionary<string, object>();
                var queryP = new Neo4jClient.Cypher.CypherQuery("start n=node(*) match (n)<-[r:sadrzi]-(a) where ID(a)="+praviId+" return n",
                                                            queryDictP, CypherResultMode.Set);
                List<Pice> kartaPica = ((IRawGraphClient)client).ExecuteGetCypherResults<Pice>(queryP).ToList();

                Dictionary<string, object> queryDictJ = new Dictionary<string, object>();
                var queryJ = new Neo4jClient.Cypher.CypherQuery("start n=node(*) match (n)<-[r:sadrzi_jelo]-(a) where ID(a)=" + praviId + " return n",
                                                            queryDictJ, CypherResultMode.Set);
                List<Jelo> kartaJela = ((IRawGraphClient)client).ExecuteGetCypherResults<Jelo>(queryJ).ToList();

                Jelovnik novaForma = new Jelovnik(kartaPica,kartaJela);
                novaForma.Show();
                //trazeni.pica = new List<string>();
                //foreach (String s in trazeni.pica)
                // {
                //   MessageBox.Show(s);
                // }

                // Jelovnik novaForma = new Jelovnik(trazeni.jela, trazeni.pica);
                //novaForma.ShowDialog();


            }
            else MessageBox.Show("Niste selektovali ni jedan restoran.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                String idRestoran = this.listView1.SelectedItems[0].SubItems[0].Text;

                int praviId = Int32.Parse(idRestoran);

                var query = new Neo4jClient.Cypher.CypherQuery("match (n:Restoran) where ID(n) =" + praviId + " return n",
                                                         new Dictionary<string, object>(), CypherResultMode.Set);

                Restoran trazeni = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query).FirstOrDefault();

                DodajKomentar novaForma = new DodajKomentar(trazeni, globalnaMusterija, praviId);
                novaForma.client = client;
                novaForma.ShowDialog();


            }
            else MessageBox.Show("Niste selektovali ni jedan restoran.");


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                String idRestoran = this.listView1.SelectedItems[0].SubItems[0].Text;

                int praviId = Int32.Parse(idRestoran);

                var query = new Neo4jClient.Cypher.CypherQuery("match (n:Restoran) where ID(n) =" + praviId + " return n",
                                                         new Dictionary<string, object>(), CypherResultMode.Set);

                Restoran trazeni = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query).FirstOrDefault();

                Komentari novaForma = new Komentari(trazeni, globalnaMusterija, praviId);
                novaForma.client = client;
                novaForma.ShowDialog();


            }
            else MessageBox.Show("Niste selektovali ni jedan restoran.");
        }

        private void MusterijaEkran_Load(object sender, EventArgs e)
        {
            this.popuniEkran();
        }

        public void popuniEkran()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Restoran) return p",
                 queryDict, CypherResultMode.Set);
            List<Restoran> restorani = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query1).ToList();
            //MessageBox.Show(restorani[0].naziv);
            List<int> idbrojevi = new List<int>();
            foreach (Restoran r in restorani)
            {
                Dictionary<string, object> queryDict0 = new Dictionary<string, object>();
                var query0 = new Neo4jClient.Cypher.CypherQuery("MATCH(r:Restoran) where r.naziv='" + r.naziv + "'" + "return ID(r)"
                    , queryDict0, CypherResultMode.Set);


                int idbroj = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query0).FirstOrDefault();

                idbrojevi.Add(idbroj);

            }
            int i = 0;
            foreach (Restoran r in restorani)
            {
                add(idbrojevi[i].ToString(), r.naziv, r.lokacija, r.brmesta);
                i++;
            }
        }
    }
}
