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
        List<Cidade> rotaSelecionada;

        SolidBrush preenchimento = new SolidBrush(Color.Black);
        Pen caneta = new Pen(Color.Red, 3);

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
                List<List<CaminhoEntreCidades>> rotas = calc.Calcular(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
                rotaSelecionada = new List<Cidade>();

                if (rotas.Count > 0)
                {
                    List<CaminhoEntreCidades> melhorCaminho = new List<CaminhoEntreCidades>();
                    dgvCaminhos.RowCount = 0;
                    dgvCaminhos.ColumnCount = 0;
                    dgvMelhorCaminho.ColumnCount = 1;
                    dgvMelhorCaminho.RowCount = 1;
                    int menor = int.MaxValue;

                    foreach (List<CaminhoEntreCidades> l in rotas)
                    {
                        dgvCaminhos.RowCount++;
                        int andado = 0, colunaAtual = 0;

                        if (l.Count() + 1 > dgvCaminhos.ColumnCount)
                            dgvCaminhos.ColumnCount = l.Count() + 1;

                        foreach (CaminhoEntreCidades dado in l)
                        {
                            andado += dado.Distancia;
                            dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(dado.Origem)).ToString();
                        }

                        dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).ToString();

                        if (andado < menor)
                        {
                            menor = andado;
                            melhorCaminho = l;
                        }
                    }

                    foreach (CaminhoEntreCidades dado in melhorCaminho)
                    {
                        Cidade atual = cidades.Buscar(new Cidade(dado.Origem));
                        dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = atual.ToString();
                        dgvMelhorCaminho.ColumnCount++;
                    }

                    dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).ToString();
                    GerarListaParaMostrar(dgvMelhorCaminho, 0); //Mostraremos o melhor caminho como padrão
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

        private void LerDeArquivo(bool ehCidade)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK) //Descobriremos o caminho do arquivo desejado
            {
                StreamReader arq = new StreamReader(dlgAbrir.FileName);

                try
                {
                    if (!ehCidade)
                    {
                        while (!arq.EndOfStream)
                        {
                            CaminhoEntreCidades c = new CaminhoEntreCidades(arq.ReadLine());
                            caminhos[c.Destino, c.Origem] = c;
                        }
                    }
                    else
                    {
                        while (!arq.EndOfStream)
                        {
                            Cidade c = new Cidade(arq.ReadLine());
                            cidades.Incluir(c);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Por favor, selecione um arquivo válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LerDeArquivo(ehCidade);
                }

                arq.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cidades = new ArvoreBinaria<Cidade>();
            dlgAbrir.Title = "Ler cidades de: ";

            LerDeArquivo(true);

            dlgAbrir.Title = "Ler caminhos de: ";
            caminhos = new CaminhoEntreCidades[cidades.QuantosFilhos, cidades.QuantosFilhos];

            LerDeArquivo(false);

            calc = new CalculadoraDeRotas(cidades.QuantosFilhos, caminhos);
            rotaSelecionada = new List<Cidade>();

            cidades.ExecutaEmTodos((Cidade c) =>
            {
                string mostrante = c.ToString();

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
            int tamanhoEsfera = Convert.ToInt32(35 * redimenLargura);

            cidades.ExecutaEmTodos((Cidade c) =>
            {
                int x = Convert.ToInt32(c.X * redimenLargura);
                int y = Convert.ToInt32(c.Y * redimenAltura);

                grafs.FillEllipse(preenchimento, x, y, tamanhoEsfera, tamanhoEsfera);

                string nome = c.Nome;

                grafs.DrawString(nome, new Font("Courier New", tamanhoEsfera, FontStyle.Bold),
                              new SolidBrush(Color.Black), x - (nome.Length * 4), y - 20);
            });

            int centroPonto = tamanhoEsfera / 2;
            void DesenharLinha(int xIni, int yIni, int xFim, int yFim)
            {
                grafs.DrawLine(caneta, Convert.ToInt32(xIni * redimenLargura) + centroPonto, Convert.ToInt32(yIni * redimenAltura) + centroPonto,
                        Convert.ToInt32(xFim * redimenLargura) + centroPonto, Convert.ToInt32(yFim * redimenAltura) + centroPonto);
            }

            for (int i = 0; i < rotaSelecionada.Count - 1; i++)
            {
                Cidade origem = rotaSelecionada[i];
                Cidade destino = rotaSelecionada[i + 1];

                if ((origem.Nome == "Arrakeen" || origem.Nome == "Senzeni Na") && destino.Nome == "Gondor") //Dois casos especiais
                {
                    int yNovo = 0;

                    if (origem.Nome == "Arrakeen")
                        yNovo = 1000;
                    else //Se não é Arrakeen, logo deve ser Senzeni Na
                        yNovo = 1350;

                    DesenharLinha(origem.X, origem.Y, -50, yNovo);
                    DesenharLinha(4096, yNovo, destino.X, destino.Y);
                }
                else
                    DesenharLinha(origem.X, origem.Y, destino.X, destino.Y);
            }
        }

        private void dgvCaminhos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            GerarListaParaMostrar(dgvCaminhos, e.RowIndex);
            pbMapa.Invalidate();
        }

        private void dgvMelhorCaminho_Click(object sender, EventArgs e)
        {
            GerarListaParaMostrar(dgvMelhorCaminho, 0); //0 pois só haverá uma linha
            pbMapa.Invalidate();
        }

        private void GerarListaParaMostrar(DataGridView dgv, int linha)
        {
            rotaSelecionada = new List<Cidade>();

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv[i, linha].Value == null)
                    break;

                string mostrando = dgv[i, linha].Value.ToString();
                Cidade busca = new Cidade(int.Parse(mostrando.Substring(0, mostrando.IndexOf('-'))));
                Cidade lida = cidades.Buscar(busca);
                rotaSelecionada.Add(lida);
            }
        }
    }
}
