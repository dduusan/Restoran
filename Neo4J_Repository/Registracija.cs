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
    public partial class Registracija : Form
    {
        Vlasnik globalniVlasnik;
        Musterija globalnaMusterija;
        Boolean vlasnik;
        GraphClient client;



        public Registracija()
        {
            InitializeComponent();
        }
        public Registracija (Vlasnik v, GraphClient klijent)
        {
            InitializeComponent();
            client = klijent;
            globalniVlasnik = v;
            vlasnik = true;
            
            
            
        }

        public Registracija(Musterija m, GraphClient klijent)
        {
            client = klijent;
            InitializeComponent();
            globalnaMusterija = m;
            vlasnik = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (vlasnik == true)
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (ee:Vlasnik) WHERE ee.email = '" + txtUserName.Text + "' RETURN ee",
                                                                queryDict, CypherResultMode.Set);

                Vlasnik fm = ((IRawGraphClient)client).ExecuteGetCypherResults<Vlasnik>(query).FirstOrDefault();
                //MessageBox.Show(fm.ime);

                if (fm != null)
                {
                    if (fm.sifra == txtPass.Text)
                    {
                        EkranVlasnika newForm = new EkranVlasnika(fm);
                        newForm.client = client;
                        newForm.ShowDialog();
                    }
                    else MessageBox.Show("pogresili ste password");
                }
                else MessageBox.Show("Nije tacan username");
            }
                
            else
            {
                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (ee:Musterija) WHERE ee.email = '" + txtUserName.Text + "' RETURN ee",
                                                                queryDict, CypherResultMode.Set);

                Musterija fm = ((IRawGraphClient)client).ExecuteGetCypherResults<Musterija>(query1).FirstOrDefault();
                
                if (fm != null)
                {
                    if (fm.sifra == txtPass.Text)
                    {
                        MusterijaEkran newForm = new MusterijaEkran(fm);
                        newForm.client = client;
                        newForm.ShowDialog();
                    }
                    else MessageBox.Show("pogresili ste password");
                }
                else MessageBox.Show("Nije tacan username");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(vlasnik == true)
            {
                //Vlasnik vlas = new Vlasnik();
                globalniVlasnik.ime = txtIme.Text;
                globalniVlasnik.prezime = txtPrez.Text;
                globalniVlasnik.email = txtEmail.Text;
                globalniVlasnik.sifra = txtPassword.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", globalniVlasnik.ime);
                queryDict.Add("prezime", globalniVlasnik.prezime);
                queryDict.Add("email", globalniVlasnik.email);
                queryDict.Add("sifra", globalniVlasnik.sifra);

                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Vlasnik {ime:'" + globalniVlasnik.ime
                                                                + "',prezime:'" + globalniVlasnik.prezime + "', email:'" + globalniVlasnik.email
                                                                + "',sifra:'" + globalniVlasnik.sifra
                                                                + "'}) return n",
                                                                queryDict, CypherResultMode.Set);


                Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
                var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (ee:Vlasnik) WHERE ee.email = '" + txtEmail.Text + "' RETURN ee.email;",
                                                                queryDict1, CypherResultMode.Set);
                String fName = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query1).FirstOrDefault();
                if (fName == null)
                {
                    ((IRawGraphClient)client).ExecuteCypher(query);
                  //  Vlasnik fm = ((IRawGraphClient)client).ExecuteGetCypherResults<Vlasnik>(query).FirstOrDefault();
                    EkranVlasnika newForm = new EkranVlasnika(globalniVlasnik);
                    newForm.client = client;
                    newForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Neko je vec registrovan kao " + txtEmail.Text + ", molim vas unesite drugi username");
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    fName = null;
                }
            }
            else
            {
              
                globalnaMusterija.ime = txtIme.Text;
                globalnaMusterija.prezime = txtPrez.Text;
                globalnaMusterija.email = txtEmail.Text;
                globalnaMusterija.sifra = txtPassword.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", globalnaMusterija.ime);
                queryDict.Add("prezime", globalnaMusterija.prezime);
                queryDict.Add("email", globalnaMusterija.email);
                queryDict.Add("sifra", globalnaMusterija.sifra);

                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Musterija {ime:'" + globalnaMusterija.ime
                                                                + "',prezime:'" + globalnaMusterija.prezime + "', email:'" + globalnaMusterija.email
                                                                + "',sifra:'" + globalnaMusterija.sifra
                                                                + "'}) return n",
                                                                queryDict, CypherResultMode.Set);


                Dictionary<string, object> queryDict1 = new Dictionary<string, object>();
                var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (ee:Musterija) WHERE ee.email = '" + txtEmail.Text + "' RETURN ee.email;",
                                                                queryDict1, CypherResultMode.Set);
                String fName = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query1).FirstOrDefault();
                if (fName == null)
                {
                    ((IRawGraphClient)client).ExecuteCypher(query);
                    MusterijaEkran newForm = new MusterijaEkran(globalnaMusterija);
                    newForm.client = client;
                    newForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Neko je vec registrovan kao " + txtEmail.Text + ", molim vas unesite drugi username");
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    fName = null;
                }
            }    
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Registracija_Load(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

