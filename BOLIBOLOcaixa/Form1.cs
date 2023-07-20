using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOLIBOLOcaixa
{
    public partial class Form1 : Form
    {
        string login, senha;
        public Form1()
        {
           
            InitializeComponent();


            login = "";//Defina um Login    
            senha = "";//defina uma senha 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loginDigitado = textBox1.Text;
            string senhaDigitada = maskedTextBox1.Text;

            if (loginDigitado == login && senhaDigitada == senha)
            {
                MessageBox.Show("Login bem-sucedido!");

                // Oculta o Form1 antes de abrir o Form2
                this.Hide();

                // Abrir o Form2
                Form2 form2 = new Form2();
                form2.ShowDialog();

                //Quando o Form2 for fechado, exibe o Form1 novamente
                this.Show();
            }
            else
            {
                MessageBox.Show("Login ou senha incorretos. Tente novamente.");
            }
        }
    }
}
