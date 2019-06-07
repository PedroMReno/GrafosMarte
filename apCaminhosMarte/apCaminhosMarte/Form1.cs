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
        CaminhoEntreCidades[,] caminhos;
        CalculadoraDeRotas calc;
        List<List<CaminhoEntreCidades>> rotas;
        List<CaminhoEntreCidades> melhorCaminho;

        SolidBrush preenchimento = new SolidBrush(Color.Black);
        Pen caneta = new Pen(Color.Black, 2);
        Pen canetaMelhor = new Pen(Color.Red, 3);

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Buscar caminhos entre cidades selecionadas");
            if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex)
                MessageBox.Show("Selecione cidades diferentes para origem e destino.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                rotas = calc.Calcular(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
                melhorCaminho = new List<CaminhoEntreCidades>();
                dgvCaminhos.RowCount = 0;
                dgvCaminhos.ColumnCount = 0;
                int menor = int.MaxValue;

                foreach (List<CaminhoEntreCidades> l in rotas)
                {
                    dgvCaminhos.RowCount++;
                    int andado = 0, colunaAtual = 0;

                    if (l.Count() + 1 > dgvCaminhos.ColumnCount)
                        dgvCaminhos.ColumnCount = l.Count() + 1;

                    foreach(CaminhoEntreCidades dado in l)
                    {
                        andado += dado.Distancia;
                        dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(dado.Origem)).Nome;
                    }

                    dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).Nome;


                    if (andado < menor)
                    {
                        menor = andado;
                        melhorCaminho = l;
                    }
                }

                if (melhorCaminho.Count > 0)
                {
                    dgvMelhorCaminho.ColumnCount = 1;
                    dgvMelhorCaminho.RowCount = 1;

                    foreach (CaminhoEntreCidades dado in melhorCaminho)
                    {
                        Cidade atual = cidades.Buscar(new Cidade(dado.Origem));
                        dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = atual.Nome;
                        dgvMelhorCaminho.ColumnCount++;
                    }

                    dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).Nome;
                }
                else
                {
                    dgvMelhorCaminho.ColumnCount = 0;
                    dgvMelhorCaminho.RowCount = 0;

                    MessageBox.Show("Nenhum caminho encontrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            pbMapa.Invalidate();
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
                    while (!arq.EndOfStream)
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
            caminhos = new CaminhoEntreCidades[cidades.QuantosFilhos, cidades.QuantosFilhos];

            if (dlgAbrir.ShowDialog() == DialogResult.OK) //Descobriremos o caminho do arquivo desejado
            {
                arq = new StreamReader(dlgAbrir.FileName);

                try
                {
                    while (!arq.EndOfStream)
                    {
                        CaminhoEntreCidades c = new CaminhoEntreCidades(arq.ReadLine());
                        caminhos[c.Destino, c.Origem] = c;
                    }
                }
                catch
                {
                    MessageBox.Show("Por favor, selecione um arquivo válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                arq.Close();
            }

            calc = new CalculadoraDeRotas(cidades.QuantosFilhos, caminhos);
            rotas = new List<List<CaminhoEntreCidades>>();
            melhorCaminho = new List<CaminhoEntreCidades>();

            cidades.ExecutaEmTodos((Cidade c) =>
            {
                string mostrante = c.Id + "- " + c.Nome;

                lsbOrigem.Items.Add(mostrante);
                lsbDestino.Items.Add(mostrante);
            });

            // coloca os selecionados do ListBox em duas cidades diferentes
            lsbOrigem.SelectedIndex = 0;
            lsbDestino.SelectedIndex = 1;
        }

        private void pbArvore_Paint(object sender, PaintEventArgs e)
        {
            cidades.DesenharArvore(e.Graphics, pbArvore.Width);
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            double redimenLargura = Math.Round((double)pbMapa.Width / 4096, 10);
            double redimenAltura = Math.Round((double)pbMapa.Height / 2048, 10);
            Graphics grafs = e.Graphics;
            int tamanhoRedimen = Convert.ToInt32(35 * redimenLargura);

            cidades.ExecutaEmTodos((Cidade c) =>
            {
                int x = Convert.ToInt32(c.X * redimenLargura);
                int y = Convert.ToInt32(c.Y * redimenAltura);

                grafs.FillEllipse(preenchimento, x, y, tamanhoRedimen, tamanhoRedimen);

                string nome = c.ToString();

                grafs.DrawString(nome, new Font("Courier New", tamanhoRedimen, FontStyle.Bold),
                              new SolidBrush(Color.Black), x - (nome.Length * 4), y - 20);
            });

            foreach(List<CaminhoEntreCidades> rota in rotas)
            {
                foreach(CaminhoEntreCidades c in rota)
                {
                    Cidade origem = cidades.Buscar(new Cidade(c.Origem));
                    Cidade destino = cidades.Buscar(new Cidade(c.Destino));

                    grafs.DrawLine(caneta, Convert.ToInt32(origem.X * redimenLargura) + tamanhoRedimen/2, Convert.ToInt32(origem.Y * redimenAltura) + tamanhoRedimen / 2,
                        Convert.ToInt32(destino.X * redimenLargura) + tamanhoRedimen / 2, Convert.ToInt32(destino.Y * redimenAltura) + tamanhoRedimen / 2);
                }
            }

            foreach (CaminhoEntreCidades c in melhorCaminho)
            {
                Cidade origem = cidades.Buscar(new Cidade(c.Origem));
                Cidade destino = cidades.Buscar(new Cidade(c.Destino));

                grafs.DrawLine(canetaMelhor, Convert.ToInt32(origem.X * redimenLargura) + tamanhoRedimen / 2, Convert.ToInt32(origem.Y * redimenAltura) + tamanhoRedimen / 2,
                    Convert.ToInt32(destino.X * redimenLargura) + tamanhoRedimen / 2, Convert.ToInt32(destino.Y * redimenAltura) + tamanhoRedimen / 2);
            }
        }
    }
}
