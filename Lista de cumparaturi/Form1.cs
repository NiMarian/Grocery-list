using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;

namespace Lista_de_cumparaturi
{
    public partial class Form1 : Form
    {
        // Dicționar pentru a stoca lista de cumpărături
        Dictionary<string, double> Cumparaturi = new Dictionary<string, double>();

        public Form1()
        {
            InitializeComponent();
        }

        //  Funcție de adăugare a unui articol în listă
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Obținem numele și prețul articolului din casetele de text
            string obiect = txtObiect.Text;
            double pret;

            // Validăm introducerea prețului
            if (!double.TryParse(txtPret.Text, out pret) || pret <= 0)
            {
                MessageBox.Show("Vă rugăm să introduceți un preț valid");
                return;
            }

            // Adăugăm articolul în listă
            if (!Cumparaturi.ContainsKey(obiect))
            {
               
                Cumparaturi.Add(obiect, pret);
               
                UpdateListBox();
                
            }
            else
            {
                MessageBox.Show("Elementul există deja în listă");
            }
           
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Obținem elementul selectat din caseta de listă
            string selectedItem = lbLista.SelectedItem as string;

            // Ne asigurăm că este selectat un articol
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            // Obținem numele articolului împărțind șirul la două puncte
            string itemName = selectedItem.Split(':')[0];

            // Eliminăm elementul din dicționar
            Cumparaturi.Remove(itemName);

            // Actualizăm caseta de listă și eticheta totală
            UpdateListBox();
        }

        private void UpdateListBox()
        {
            lbLista.Items.Clear();

            // Adăugăm fiecare articol din listă în caseta de listă
            foreach (KeyValuePair<string, double> obiect in Cumparaturi)
            {
                lbLista.Items.Add($"{obiect.Key}: ${obiect.Value}");
            }

            // Calculăm și afișăm costul total
            double totalCost = Cumparaturi.Values.Sum();
            lblTotal.Text = $"Total: ${totalCost}";
        }

        // Funcție de salvare a listei într-un fișier .txt
        void btnSave_Click(object sender, EventArgs e)
        {
            // Afișăm dialogul de salvare a fișierului
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text file|*.txt";
            saveFileDialog1.Title = "Save a Text File";
            saveFileDialog1.ShowDialog();

            // Dacă calea fișierului nu este un string gol, deschidem pentru salvare
            if (saveFileDialog1.FileName != "")
            {
                // Creăm un string builder pentru a stoca liniile de text
                StringBuilder sb = new StringBuilder();

                // Iterăm prin dicționar și adăugăm fiecare articol și preț la string builder
                foreach (KeyValuePair<string, double> item in Cumparaturi)
                {
                    sb.AppendLine($"{item.Key}: ${item.Value}");
                }

                // Scriem string builder în fișier
                File.WriteAllText(saveFileDialog1.FileName, sb.ToString());
            }
        }


        // Funcție pentru a încărca lista dintr-un fișier .txt
     
        void btnLoad_Click(object sender, EventArgs e)
        {
            // Afișăm dialogul de deschidere a fișierului
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Obținem calea fișierului selectat
                string filePath = openFileDialog1.FileName;

                // Citim conținutul fișierului
                string text = File.ReadAllText(filePath);

                // Împărțim textul în rânduri
                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                // Analizăm rândurile și adăugați articolele și prețurile în dicționar
                foreach (string line in lines)
                {
                    // Împărțim linia în părți
                    string[] parts = line.Split(':');

                    // Printăm părțile
                    Console.WriteLine($"Line: {line}");
                    Console.WriteLine($"Parts: {string.Join(", ", parts)}");

                    // Verificăm dacă există suficiente părți
                    if (parts.Length >= 2)
                    {
                        // Tăiăm spațiul alb din părțile și analizăm prețul
                        string item = parts[0].Trim();
                        double price = double.Parse(parts[1].Trim().Substring(1));

                        // Adăugăm articolul și prețul în dicționar
                        Cumparaturi.Add(item, price);
                    }
                }

                // Iterăm prin dicționar și printăm articolele și prețurile
                foreach (KeyValuePair<string, double> item in Cumparaturi)
                {
                    Console.WriteLine($"{item.Key}: ${item.Value}");
                }

                // Actualizăm caseta cu listă
                UpdateListBox();
            }
        }

    }
    
}
  
