using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BOLIBOLOcaixa
{

    //------------------------------------------------- CODIGO FORM2 ABAIXO ----------------------------------------------------

    public partial class Form2 : Form
    {
        // Lista para armazenar os itens de venda
        private List<ItemVenda> itensVenda = new List<ItemVenda>();
        private int proximoCodigo = 1; // Variável para controlar o próximo código a ser atribuído
        private bool coberturaAumentada = false;
        public Form2()
        {
            InitializeComponent();

            // Adiciona as colunas ao DataGridView
            dataGridViewVendas.Columns.Add("Codigo", "Código do Produto");
            dataGridViewVendas.Columns.Add("Nome", "Nome do Produto");
            dataGridViewVendas.Columns.Add("Preco", "Preço do Produto");

            // Configura o tamanho das colunas do DataGridView
            ConfigurarTamanhoColunas();
        }

        // Método para configurar o tamanho das colunas do DataGridView
        private void ConfigurarTamanhoColunas()
        {
            dataGridViewVendas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Defina a largura desejada para cada coluna
            dataGridViewVendas.Columns["Codigo"].Width = 65; // Exemplo: 100 pixels
            dataGridViewVendas.Columns["Nome"].Width = 222;   // Exemplo: 200 pixels
            dataGridViewVendas.Columns["Preco"].Width = 70;  // Exemplo: 150 pixels
        }

        // Evento do botão "Adicionar Produto"
        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            // Verifica se os campos de nome e preço não estão vazios e se o preço é válido
            if (!string.IsNullOrEmpty(textBoxProduto.Text) && double.TryParse(textBoxPreco.Text, out double preco))
            {
                // Cria um novo ItemVenda com o nome e codigo e o preço informados
                ItemVenda item = new ItemVenda { Codigo = proximoCodigo, Nome = textBoxProduto.Text, Preco = preco };
                // Incrementa o próximo código para o próximo item
                proximoCodigo++;
                // Adiciona o item à lista de itens de venda
                itensVenda.Add(item);
                // Atualiza a tabela de vendas
                AtualizarTabela();

                // Adiciona o nome e preço do item à DataGridView para exibir na tabela (caso nao tivesse o atualizar tabela)
                //dataGridViewVendas.Rows.Add(item.Nome, item.Preco);
            }
            else
            {
                // Caso os campos não estejam preenchidos corretamente, exibe uma mensagem de erro
                MessageBox.Show("Por favor, preencha o nome e o preço corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento do botão "Calcular Lucro"
        private void buttonCalcularLucro_Click(object sender, EventArgs e)
        {
            double lucroTotal = 0;

            // Percorre todos os itens de venda e soma os preços para calcular o lucro total
            foreach (var item in itensVenda)
            {
                lucroTotal += item.Preco;
            }

            // Exibe uma mensagem com o lucro total calculado
            MessageBox.Show($"Lucro total: R${lucroTotal:F2}", "Lucro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void AtualizarTabela()
        {
            // Limpa todas as linhas do DataGridView
            dataGridViewVendas.Rows.Clear();

            // Adiciona novamente os itens da lista ao DataGridView
            foreach (var item in itensVenda)
            {
                // Formata o preço para exibição
                string precoFormatado = item.Preco.ToString("C2");

                // Adiciona a linha à tabela de vendas, incluindo o preço formatado com "R$"
                dataGridViewVendas.Rows.Add(item.Codigo, item.Nome, precoFormatado);
            }
        }



        // Classe para representar os itens de venda
        public class ItemVenda
        {
            public int Codigo { get; set; }
            public string Nome { get; set; }
            public double Preco { get; set; }
        }

        private void buttonApagarUltimo_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na lista antes de apagar o último
            if (itensVenda.Count > 0)
            {
                // Remove apenas o último item da lista (não remove a última linha do DataGridView)
                itensVenda.RemoveAt(itensVenda.Count - 1);
                // Atualiza a tabela de vendas
                AtualizarTabela();
            }
        }

        private void buttonApagarTudo_Click(object sender, EventArgs e)
        {
            // Limpa a lista de itens de venda
            itensVenda.Clear();
            // Limpa todas as linhas do DataGridView
            dataGridViewVendas.Rows.Clear();
        }

        //------------------------------------------------- BOTÕES DAS COBERTURAS ABAIXO------------------------------------------------------------
        private void buttonExtraCob_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na lista antes de adicionar R$ 12,00 ao último item
            if (itensVenda.Count > 0)
            {
                // Aumenta a cobertura em R$ 12,00 do último item da lista
                if (itensVenda.Count > 0)
                {
                    // Obtém o último item da lista
                    ItemVenda ultimoItem = itensVenda[itensVenda.Count - 1];
                    // Adiciona R$ 10,00 ao preço do último item
                    ultimoItem.Preco += 10.00;
                    // Define a variável de controle como verdadeira
                    coberturaAumentada = true;
                    // Atualiza a tabela de vendas
                    AtualizarTabela();
                }
            }
        }

        private void buttonExtraCob2_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na lista antes de subtrair R$ 12,00 do último item
            if (itensVenda.Count > 0)
            {
                // Verifica se a cobertura foi aumentada antes de diminuir
                if (coberturaAumentada)
                {
                    if (itensVenda.Count > 0)
                    {
                        // Obtém o último item da lista
                        ItemVenda ultimoItem = itensVenda[itensVenda.Count - 1];
                        // Subtrai R$ 10,00 do preço do último item, garantindo que o preço não seja negativo
                        ultimoItem.Preco = Math.Max(ultimoItem.Preco - 10.00, 0);

                        // Atualiza a tabela de vendas
                        AtualizarTabela();
                    }
                }
                else
                {
                    MessageBox.Show("A cobertura precisa ser aumentada antes de ser diminuída.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void buttonExtraCob3_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Cobertura no pote", Preco = 12.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //------------------------------------------------- FUNÇOES VIA CODIGO ABAIXO------------------------------------------------------------

        private void buttonAlterarProduto_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na lista
            if (itensVenda.Count > 0)
            {
                // Verifica se o usuário digitou um código válido
                if (int.TryParse(textBoxCodigoProduto.Text, out int codigoProduto))
                {
                    // Procura o item na lista pelo código informado
                    ItemVenda item = itensVenda.FirstOrDefault(i => i.Codigo == codigoProduto);

                    if (item != null)
                    {
                        // Verifica se o usuário deseja alterar o nome do produto
                        if (!string.IsNullOrEmpty(textBoxNovoName.Text))
                        {
                            item.Nome = textBoxNovoName.Text;
                        }

                        // Verifica se o usuário deseja alterar o preço do produto
                        if (double.TryParse(textBoxNovoPreco.Text, out double novoPreco))
                        {
                            item.Preco = novoPreco;
                        }

                        // Atualiza a tabela de vendas
                        AtualizarTabela();
                    }
                    else
                    {
                        MessageBox.Show("Código de produto inválido. Verifique o código digitado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Código de produto inválido. Verifique o código digitado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonExcluirProduto_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na lista
            if (itensVenda.Count > 0)
            {
                // Verifica se o usuário digitou um código válido
                if (int.TryParse(textBoxCodigoProduto.Text, out int codigoProduto))
                {
                    // Procura o item na lista pelo código informado
                    ItemVenda item = itensVenda.FirstOrDefault(i => i.Codigo == codigoProduto);

                    if (item != null)
                    {
                        // Remove o item da lista de itens de venda
                        itensVenda.Remove(item);
                        // Atualiza a tabela de vendas
                        AtualizarTabela();
                    }
                    else
                    {
                        MessageBox.Show("Código de produto inválido. Verifique o código digitado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Código de produto inválido. Verifique o código digitado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //------------------------------------------------- FUNÇOES INUTILIZAVEIS ABAIXO------------------------------------------------------------

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }


        //------------------------------------------------- BOTÕES DOS BOLOS ABAIXO------------------------------------------------------------

        //-------------------------------------------------------- R$18,00 --------------------------------------------------------------------

        private void buttonAdicionarPredefinidoLARANJA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Laranja", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoLIMAO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Limão", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoCENOURA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Cenoura", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoCHOCOLATE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Chocolate", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoFUBA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Fubá normal ", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoMARACUJA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Maracuja", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoMANDIOCA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Mandioca", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoCOCO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Coco", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoFORMIGUEIRO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo Formigueiro", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoMESCLADO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo Mesclado", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoCAÇAROLA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo De Caçarola", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoFERVADOCE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo De Fubá com erva doce", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonAdicionarPredefinidoFGOIBADA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo De Fubá com goiabada", Preco = 18.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$16,00 --------------------------------------------------------------------

        private void buttonPredefinidoSoltFRUTA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho com fruta", Preco = 16.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoSoltCOBERTURA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho com cobertura", Preco = 16.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoSoltIOGURTE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho com iogurte", Preco = 16.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoSoltAMORA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho com iogurte e amora", Preco = 16.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoSoltCHURROS_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho de churros", Preco = 16.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$10,00 --------------------------------------------------------------------
        private void buttonPredefinidoSoltSIMPLES_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Solteirinho simples ", Preco = 10.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$20,00 --------------------------------------------------------------------

        private void buttonPredefinidoDietCENOURA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo diet de cenoura", Preco = 20.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoDietIOGURT_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo diet de iogurt", Preco = 20.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoIntMAÇÃ_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo integral de maçã", Preco = 20.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoIntBANANA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo integral de banana", Preco = 20.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$22,00 --------------------------------------------------------------------

        private void buttonAdicionarPredefinido1_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Milho", Preco = 22.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$28,00 --------------------------------------------------------------------
        private void buttonPredefinidoMAÇACANELA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Maçã com canela", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoBANANACANELA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Banana com canela", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoABACAXI_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Abacaxi", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoFUBACREME_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Fubá cremoso", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoPRESTIGIO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Prestigio", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoFELPUDAO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Toalha felpuda", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoPAÇOCAPAÇOCA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Paçoca com cobertura de paçoca", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoIOGURTAMORA_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Iogurte com Amora", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoBOLOCHURROSSS_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Churros", Preco = 28.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- R$58,00 --------------------------------------------------------------------
        private void buttonPredefinidoCENOURAPUDIM_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Cenoura com Pudim de chocolate", Preco = 52.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoLEITEPÓ_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Bolo de Leite em pó", Preco = 50.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //-------------------------------------------------------- ETC... --------------------------------------------------------------------
        private void buttonPredefinidoEMBALAGEM_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Embalagem", Preco = 6.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Torta de Frango", Preco = 40.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoPUDIM_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Pudim", Preco = 40.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoTORTACARNE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Torta de Carne", Preco = 45.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoTORTAPALMITO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Torta de Palmito", Preco = 45.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoTORTAOREO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Torta doce de OREO", Preco = 65.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoBANOFFE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Banoffe", Preco = 65.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoTORTADOCELIMAO_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Torta doce de Limão", Preco = 65.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoCOCAMINI_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Coca-Cola pequena", Preco = 3.50 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoCOCAGRANDE_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Coca-Cola grande", Preco = 15.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoSUQUIN_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Suco pequeno", Preco = 3.50 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoREFRIS_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Outros Refrigerantes", Preco = 12.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        private void buttonPredefinidoDOCES_Click(object sender, EventArgs e)
        {
            // Criar um item de venda predefinido com nome e preço já definidos
            ItemVenda itemPredefinido = new ItemVenda { Codigo = proximoCodigo, Nome = "Doces", Preco = 2.00 };
            // Incrementar o próximo código para o próximo item
            proximoCodigo++;
            // Adicionar o item predefinido à lista de itens de venda
            itensVenda.Add(itemPredefinido);
            // Atualizar a tabela de vendas
            AtualizarTabela();
        }

        //------------------------------------------------- BOTÕES DE SALVAR TABELA------------------------------------------------------------

        private void buttonSalvarTabela_Click(object sender, EventArgs e)
        {
            // Verifica se há itens na tabela
            if (dataGridViewVendas.Rows.Count > 1)
            {
                // Cria o objeto SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Arquivo CSV (*.csv)|*.csv";
                saveFileDialog.Title = "Salvar Tabela";

                // Exibe a caixa de diálogo para o usuário escolher o local e o nome do arquivo
                DialogResult result = saveFileDialog.ShowDialog();

                // Verifica se o usuário selecionou um local válido
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    try
                    {
                        // Abre o arquivo para escrita
                        using (var writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                        {
                            // Escreve o cabeçalho da tabela
                            string header = string.Join(",", dataGridViewVendas.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText));
                            writer.WriteLine(header);

                            // Escreve os dados da tabela
                            foreach (DataGridViewRow row in dataGridViewVendas.Rows)
                            {
                                string rowData = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString()));
                                writer.WriteLine(rowData);
                            }
                        }

                        MessageBox.Show("Tabela salva com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao salvar a tabela: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Não há itens na tabela para salvar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}



