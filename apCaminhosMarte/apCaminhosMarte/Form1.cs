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
        // Armazenamento das cidades do arquivo texto
        ArvoreBinaria<Cidade> cidades;

        //Armazenamento dos caminhos do arquivo texto
        CaminhoEntreCidades[,] caminhos;

        //Armazenamento da menor rota calculada
        CalculadoraDeRotas calc;

        //Armazenamento da lista de cidades (rota) selecionada
        List<Cidade> rotaSelecionada;

        //Variaveis usadas para desenhar no PictureBox
        SolidBrush preenchimento = new SolidBrush(Color.Black);
        Pen caneta = new Pen(Color.Red, 3);

        public Form1()
        {
            InitializeComponent();
        }

        //Codificacao do botao de busca de cainhos entre origem e destino selecionados
        private void BtnBuscar_Click(object sender, EventArgs e) 
        {
            if (lsbOrigem.SelectedIndex == lsbDestino.SelectedIndex) //Caso origem e destino sejam iguais, lanca uma excessao
                MessageBox.Show("Selecione cidades diferentes para origem e destino.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                //Lista de armazenamento de todos os caminhos para que seja alcancado o destino desejado
                List<List<CaminhoEntreCidades>> rotas = calc.Calcular(lsbOrigem.SelectedIndex, lsbDestino.SelectedIndex);
                rotaSelecionada = new List<Cidade>(); //Instaciacao da rota selecionada

                if (rotas.Count > 0)
                {
                    List<CaminhoEntreCidades> melhorCaminho = new List<CaminhoEntreCidades>(); //Lista de cidades componentes do menor caminho encontrado
                    dgvCaminhos.RowCount = 0; // zerando as linhas do grid
                    dgvCaminhos.ColumnCount = 0; // zerando as colunas do grid

                    dgvMelhorCaminho.ColumnCount = 1; // iniciando o grid que armazena o melhor caminho
                    dgvMelhorCaminho.RowCount = 1;

                    int menor = int.MaxValue;

                    foreach (List<CaminhoEntreCidades> l in rotas) // percorrendo a lista que contem a lista de caminhos
                    {
                        dgvCaminhos.RowCount++; // adiciona uma linha ao grid
                        int andado = 0, colunaAtual = 0; // variaveis locis para adicionar as cidades no grid

                        if (l.Count() + 1 > dgvCaminhos.ColumnCount) // se a quantidade de cidades for maior que a quantidade de colunas da tabela
                            dgvCaminhos.ColumnCount = l.Count() + 1; // adiciona as colunas necessarias

                        foreach(CaminhoEntreCidades dado in l) // percorre os caminhos da lista
                        {
                            andado += dado.Distancia; // soma a distancia a ja percorrida
                            //escreve o ID e o nome da cidade no grid depois de busca-la na arvore
                            dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(dado.Origem)).ToString(); 
                        }

                         // escreve o destino final no grid
                        dgvCaminhos[colunaAtual++, dgvCaminhos.RowCount - 1].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).ToString();

                        if (andado < menor) //caso caminho percorrido seja menor que o ultimo que foi armazenado, ele  guardado
                        {
                            menor = andado;
                            melhorCaminho = l;
                        }
                    }

                    foreach (CaminhoEntreCidades dado in melhorCaminho) //percorre todos os caminhos da menor rota encontrada 
                    {
                        Cidade atual = cidades.Buscar(new Cidade(dado.Origem)); //busca a cidade de origem do caminho em questao na arvore
                        dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = atual.ToString(); // escreve a ciadade no grid
                        dgvMelhorCaminho.ColumnCount++; //diciona mais uma coluna ao grid
                    }

                    // escreve o destino final no grid
                    dgvMelhorCaminho[dgvMelhorCaminho.ColumnCount - 1, 0].Value = cidades.Buscar(new Cidade(lsbDestino.SelectedIndex)).ToString();
                    GerarListaParaMostrar(dgvMelhorCaminho, 0); //Mostraremos o melhor caminho como padrão
                }
                else //se o melhor caminho nao foi encontrado 
                { 
                    // zera o grid que o marmazenaria 
                    dgvMelhorCaminho.ColumnCount = 0; 
                    dgvMelhorCaminho.RowCount = 0;

                    // lanca uma excessao de caminho nao encontrado
                    MessageBox.Show("Nenhum caminho encontrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            pbMapa.Invalidate(); // chama o refresh do PictureBox
        }

        private void LerDeArquivo(bool ehCidade) // metodo usado para ler os dois arquivos que apenas recebe o boolean que indica se e cidade
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK) //Descobriremos o caminho do arquivo desejado
            {
                StreamReader arq = new StreamReader(dlgAbrir.FileName); // abertura do arquivo

                try
                {
                    if (!ehCidade) // se for um arquivo de caminhos entre cidades
                    {
                        while (!arq.EndOfStream) // enquanto o arquivo nao terminou, percorremos 
                        {
                            CaminhoEntreCidades c = new CaminhoEntreCidades(arq.ReadLine()); // le o caminho do arquivo
                            caminhos[c.Destino, c.Origem] = c; // armazena o caminho na matriz
                        }
                    }
                    else // se for um arquivo de cidades
                    {
                        while (!arq.EndOfStream) // enquanto o arquivo nao terminou, percorremos 
                        {
                            Cidade c = new Cidade(arq.ReadLine()); // le a cidade do arquivo
                            cidades.Incluir(c); // armazena a cidade na arvore 
                        }
                    }
                }
                catch // caso arquivo nao esteja nos padroes de dados pre definidos, informa o usuario e chama novamente o metodo
                {
                    MessageBox.Show("Por favor, selecione um arquivo válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LerDeArquivo(ehCidade);
                }

                arq.Close(); // fecha o arquivo lido
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cidades = new ArvoreBinaria<Cidade>(); // instancia a arvore
            dlgAbrir.Title = "Ler cidades de: "; // coloca um titulo no dialog

            LerDeArquivo(true); // chama o metodo de leitura de arquivo

            dlgAbrir.Title = "Ler caminhos de: "; // coloca outro titulo no mesmo dialog
            caminhos = new CaminhoEntreCidades[cidades.QuantosFilhos, cidades.QuantosFilhos]; //instancia a matriz com a quiantidade de elementos da arvore

            LerDeArquivo(false); //chama o metodo de leitura de arquivo

            calc = new CalculadoraDeRotas(cidades.QuantosFilhos, caminhos); //instancia a calculadora de rotas com os caminhos e a quantidade de elementos da arvore
            rotaSelecionada = new List<Cidade>(); // instancia a rota selecionada

            cidades.ExecutaEmTodos((Cidade c) => //chama o metodo da arvore que recebe uma funcao como parametro e excutara essa funcao em todos os nos da arvore
            {
                string mostrante = c.ToString(); // recebe o nome da cidade

                lsbOrigem.Items.Add(mostrante); // escrve no listBox de origem
                lsbDestino.Items.Add(mostrante);  // escrve no listBox de destino
            });

            // coloca os selecionados do ListBox em duas cidades diferentes
            lsbOrigem.SelectedIndex = 0;
            lsbDestino.SelectedIndex = 1;
        }

        private void pbArvore_Paint(object sender, PaintEventArgs e)
        {
            cidades.DesenharArvore(e.Graphics, pbArvore.Width); // desenha a arvore com as cidades
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e) //desenhar no mapa presente no pictureBox
        {
            //instancia numeros base para o redimensionamento da imagem
            double redimenLargura = Math.Round((double)pbMapa.Width / 4096, 10);
            double redimenAltura = Math.Round((double)pbMapa.Height / 2048, 10);

            Graphics grafs = e.Graphics;
            int tamanhoEsfera = Convert.ToInt32(35 * redimenLargura);

            cidades.ExecutaEmTodos((Cidade c) => // chama o metodo em todos os nos da arvore 
            {
                // redimensiona as coordenadas da cidade de acordo com o tamanho do mapa
                int x = Convert.ToInt32(c.X * redimenLargura); 
                int y = Convert.ToInt32(c.Y * redimenAltura);

                grafs.FillEllipse(preenchimento, x, y, tamanhoEsfera, tamanhoEsfera); // desenha um circulo nas coordenadas redimenionadas da cidade

                string nome = c.Nome; // guarda o nome da cidade 

                grafs.DrawString(nome, new Font("Courier New", tamanhoEsfera, FontStyle.Bold),
                              new SolidBrush(Color.Black), x - (nome.Length * 4), y - 20); // escreve o nome da cidade no mapa
            });

            int centroPonto = tamanhoEsfera / 2;
            void DesenharLinha(int xIni, int yIni, int xFim, int yFim)
            {
                grafs.DrawLine(caneta, Convert.ToInt32(xIni * redimenLargura) + centroPonto, Convert.ToInt32(yIni * redimenAltura) + centroPonto,
                        Convert.ToInt32(xFim * redimenLargura) + centroPonto, Convert.ToInt32(yFim * redimenAltura) + centroPonto);
            }

            for (int i = 0; i < rotaSelecionada.Count - 1; i++) // percorre a rota selecionada 
            {
                Cidade origem = rotaSelecionada[i]; // armazena a "cidade atual"na lista (for) 
                Cidade destino = rotaSelecionada[i + 1];// armazena a proxima cidade

                if ((origem.Nome == "Arrakeen" || origem.Nome == "Senzeni Na") && destino.Nome == "Gondor") //Caso seja um dos dois casos especiais
                {
                    int yNovo = 0; // variavel local na nova coordenada y

                    if (origem.Nome == "Arrakeen")
                        yNovo = 1000;
                    else //Se não é Arrakeen, logo deve ser Senzeni Na
                        yNovo = 1350;

                    DesenharLinha(origem.X, origem.Y, -50, yNovo); // desenha a linha da cidade de rigem ate o novo ponto (na lateral do mapa para se liga a outra cidade na lateral oposta)
                    DesenharLinha(4096, yNovo, destino.X, destino.Y); // desenha a linha do novo ponto ate a cidade de destino (na lateral do mapa para se liga a outra cidade na lateral oposta)
                }
                else // se nao for, desenhamos a linha entre as cidades normalmente
                    DesenharLinha(origem.X, origem.Y, destino.X, destino.Y);
            }
        }

        private void dgvCaminhos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            GerarListaParaMostrar(dgvCaminhos, e.RowIndex); //mostra o caminho selecionado
            pbMapa.Invalidate(); // força o paint do mapa

        }

        private void dgvMelhorCaminho_Click(object sender, EventArgs e)
        {
            GerarListaParaMostrar(dgvMelhorCaminho, 0); //mostra o melhor caminho; 0 pois só haverá uma linha
            pbMapa.Invalidate(); // força o paint do mapa

        }

        private void GerarListaParaMostrar(DataGridView dgv, int linha)
        {
            rotaSelecionada = new List<Cidade>(); // inicializa a rota selecionada

            for (int i = 0; i < dgv.ColumnCount; i++) // percorre o grid especificado 
            {
                if (dgv[i, linha].Value == null)
                    break;

                string mostrando = dgv[i, 0].Value.ToString(); // guarda o ID da cidade 
                Cidade lida = cidades.Buscar(new Cidade(int.Parse(mostrando.Substring(0, mostrando.IndexOf('-'))))); // armazena o retorno da busca na arvore
                rotaSelecionada.Add(lida); // adiciona a cidade na rota
            }
        }
    }
}