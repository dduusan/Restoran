using Neo4J_Repository.DomainModel;
using Neo4jClient;
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
    public partial class Login : Form
    {
       public GraphClient client;
        public Vlasnik noviVlasnik;
        public Musterija novaMusterija;
        public Login()
        {
            
            InitializeComponent();
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "restorani2");
            try
            {
                client.Connect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            noviVlasnik = new Vlasnik();
            Registracija logovanje = new Registracija(noviVlasnik, client);
            logovanje.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            novaMusterija = new Musterija();
            Registracija logovanje = new Registracija(novaMusterija, client);
            logovanje.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
