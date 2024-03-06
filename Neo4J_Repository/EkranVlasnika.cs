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
    public partial class EkranVlasnika : Form
    {
        public Vlasnik globalniVlasnik; 
        public GraphClient client;
        public List<Restoran> listaRestorana; 
        public EkranVlasnika()
        {
            InitializeComponent();
        }
        public EkranVlasnika(Vlasnik v)
        {
            InitializeComponent();
            globalniVlasnik = v;
             this.popuniInicijalno();
           
            String mejl = globalniVlasnik.email;
            MessageBox.Show(mejl);
           
                                                          

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            DodajRestoran dodavanje = new DodajRestoran();
            dodavanje.client = client;
            dodavanje.globalniVlasnik = globalniVlasnik;
            dodavanje.ShowDialog();
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
            String[] row = { idR, naziv, lokacija, brojmesta };
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                String idRestoran = this.listView1.SelectedItems[0].SubItems[0].Text;
                
                int praviId = Int32.Parse(idRestoran);
               // MessageBox.Show(praviId.ToString());
                var query = new Neo4jClient.Cypher.CypherQuery("match (n:Restoran) where ID(n) ="+ praviId + " OPTIONAL MATCH(n)-[r] - () delete r, n",
                                                         new Dictionary<string, object>(), CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);
                MessageBox.Show("Restoran je obrisan!");
                this.refresujEkran();
                
            }
        }
        public void refresujEkran()
        {
            listView1.Clear();
            this.popuniInicijalno();
            this.popuniEkran();
        }
        private void EkranVlasnika_Load(object sender, EventArgs e)
        {

            this.popuniEkran();

        }
        public void popuniEkran()
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            var query1 = new Neo4jClient.Cypher.CypherQuery("match (n)<-[r:je_vlasnik]-(a) where exists(a.email) and a.email=" + "'" + globalniVlasnik.email + "'" + " return n",
                 queryDict, CypherResultMode.Set);
            List<Restoran> restorani = ((IRawGraphClient)client).ExecuteGetCypherResults<Restoran>(query1).ToList();
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
