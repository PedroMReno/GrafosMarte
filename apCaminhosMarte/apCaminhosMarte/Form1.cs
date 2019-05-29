using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        ArvoreBinaria<Cidade> cidades;

        public Form1()
        {
            InitializeComponent();
            lsbOrigem.SelectedIndex = 0;
            lsbDestino.SelectedIndex = 1;
        }

        public void AjustarGrid(DataGridView gv, int qtdL, int qtdC)
        {
            //Descobrindo quais serão as dimensões das células do gridview conforme o número de colunas e linhas
            int largura = gv.Width / qtdC;
            int altura = gv.Height / qtdL;

            if (largura < 20 || altura < 20) //Caso as dimensões fiquem muito pequenas, faremos com que elas tenham um 
                                             //tamanho aceitável e acionaremos as scrollBars
            {
                largura = altura = 20;
                gv.ScrollBars = ScrollBars.Both;
            }
            else //Caso contrário, utizaremos os valores calculados e esconderemos as ScrollBars
                gv.ScrollBars = ScrollBars.None;

            //Ajustaremos as dimensões das células do GridView em questão
            foreach (DataGridViewColumn c in gv.Columns)
                c.Width = largura;

            foreach (DataGridViewRow a in gv.Rows)
                a.Height = altura;
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Buscar caminhos entre cidades selecionadas");
            if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex)
                MessageBox.Show("Selecione cidades diferentes para origem e destino.");
            else
            {

                //AjustarGrid(dgvCaminhos, dgvCaminhos.RowCount, dgvCaminhos.ColumnCount);
                //AjustarGrid(dgvMelhorCaminho, dgvMelhorCaminho.RowCount, dgvMelhorCaminho.ColumnCount);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cidades = new ArvoreBinaria<Cidade>();

            dlgAbrir.Title = "Ler cidades de: ";

            StreamReader arq;

            if (dlgAbrir.ShowDialog() == DialogResult.OK) //Descobriremos o caminho do arquivo desejado
            {
                arq = new StreamReader(dlgAbrir.FileName);

                try
                {
                    while(!arq.EndOfStream)
                    {
                        Cidade c = new Cidade(arq.ReadLine());
                        cidades.Incluir(c);
                    }
                }
                catch
                {
                    MessageBox.Show("Por favor, selecione um arquivo válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                arq.Close();
            }

            dlgAbrir.Title = "Ler caminhos de: ";

            if (dlgAbrir.ShowDialog() == DialogResult.OK) //Descobriremos o caminho do arquivo desejado
            {
                arq = new StreamReader(dlgAbrir.FileName);

                try
                {
                    while (!arq.EndOfStream)
                    {
                        CaminhoEntreCidades c = new CaminhoEntreCidades(arq.ReadLine());
                        //icluir caminhos na matriz
                    }
                }
                catch
                {
                    MessageBox.Show("Por favor, selecione um arquivo válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                arq.Close();
            }

            pbMapa.Invalidate();
        }

        private void pbArvore_Paint(object sender, PaintEventArgs e)
        {
            cidades.DesenharArvore(e.Graphics, pbArvore.Width);
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            var grafs = pbMapa.CreateGraphics();

            grafs.Clear(Color.White);

            SolidBrush preenchimento = new SolidBrush(Color.BlueViolet);
            grafs.FillEllipse(preenchimento, 15, 13, 30, 30);
        }
    }
}
