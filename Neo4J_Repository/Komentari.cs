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
    public partial class Komentari : Form
    {
        public GraphClient client;
        public Restoran globalniRestoran;
        public Musterija globalnaMusterija;
        public int selektovaniRes;
        public Komentari()
        {
            InitializeComponent();
        }
        public Komentari(Restoran r, Musterija m, int idR)
        {
            
            InitializeComponent();
            globalniRestoran = r;
            globalnaMusterija = m;
            selektovaniRes = idR;
        }
        public void prikaziKomentare()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Komentari za restoran: " + globalniRestoran.naziv, 300);
            listView1.Columns.Add("OCENA:" ,150);
            listView1.Columns.Add("KORISNIK:", 150);

            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) match (n)-[r:za]->(b:Restoran) " +
                "where ID(b)=" +selektovaniRes+" return n", new Dictionary<string, object>(), CypherResultMode.Set);
            List<Komentar> komentari = new List<Komentar>();
            komentari = ((IRawGraphClient)client).ExecuteGetCypherResults<Komentar>(query).ToList();
            Musterija user = new Musterija();

            foreach (Komentar kom in komentari)
            {
                var query1 = new Neo4jClient.Cypher.CypherQuery("start n=node(*) match (n)-[r:je_komentarisao]->(b:Komentar) " +
                "where b.poruka='" + kom.poruka + "'" + " return n", new Dictionary<string, object>(), CypherResultMode.Set);
                user = ((IRawGraphClient)client).ExecuteGetCypherResults<Musterija>(query1).FirstOrDefault();
                addK(kom.poruka, kom.ocena, user.email);
            }
        }
        public void addK(string poruka, int ocenaR, string user)
        {
            String[] row = { poruka, ocenaR.ToString(), user};
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Komentari_Load(object sender, EventArgs e)
        {
            this.prikaziKomentare();
        }
    }
}
