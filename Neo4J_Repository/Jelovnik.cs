using Neo4J_Repository.DomainModel;
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
    public partial class Jelovnik : Form
    {
       public List<Pice> picenca;
        public List<Jelo> jelence;
        public Jelovnik()
        {
            InitializeComponent();
        }
        public Jelovnik(List<Pice> kPica, List<Jelo> kJela)
        {
            InitializeComponent();
            picenca = kPica;
            jelence = kJela;
          
        }
        public void printInicijalno(List<Pice> kPica, List<Jelo> kJela)
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Naziv Pica", 100);
           
            listView2.View = View.Details;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("Naziv Jela", 100);
          
            foreach (Jelo j in kJela)
            {
                add2(j.ime);
            }
            foreach(Pice p in kPica)
            {
                add1(p.ime);
            }
        }
        public void add1(string naziv)
        {
            String[] row = { naziv };
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }
        public void add2(string naziv)
        {
            String[] row = { naziv };
            ListViewItem item = new ListViewItem(row);
            listView2.Items.Add(item);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Jelovnik_Load(object sender, EventArgs e)
        {
            this.printInicijalno(picenca, jelence);
        }
    }
}
