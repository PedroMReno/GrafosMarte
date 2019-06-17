//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;
using System.Drawing;

class ArvoreBinaria<Dado> where Dado : IComparable<Dado>, IParaArvore //Pode ter menos métodos
{
    private NoArvore<Dado> raiz, atual, antecessor;
    private int nos;

    public ArvoreBinaria() //Construtor básico
    {
        raiz = atual = antecessor = null;
    }

    public int QuantosNos
    {
        get => nos;
    } //Retorna a quantidade de nós na árvore

    public Dado Buscar(Dado modelo)
    {
        Dado ret = default(Dado); //Caso nada seja achado, retornaremos o default de Dado

        if (ExisteDado(modelo)) //Procuraremos se o dado existe
            return atual.Info; //Se sim, retornaremos o dado que achamos

        return ret;
    }

    public bool ExisteDado(Dado procurado)
    {
        //Inicializando as variáveis
        antecessor = null;
        atual = raiz;

        while (atual != null) //Enquanto não passamos das folhas
        {
            if (atual.Info.CompareTo(procurado) == 0) //Retornaremos verdadeiro caso achamos
                return true;
            else
            {
                antecessor = atual; //Antecessor armazenará o antigo valor de atual

                if (procurado.CompareTo(atual.Info) < 0) //Caso atual seja maior
                    atual = atual.Esq;     // Desloca à esquerda
                else //Caso atual seja menor
                    atual = atual.Dir;      // Desloca à direita
            }
        }
        return false;       // Se atual == null, a chave não existe
    }

    public void Incluir(Dado dadoLido)
    {
        if (ExisteDado(dadoLido)) //Caso o dado já exista
            throw new Exception("Dado já existente"); //Avisaremos o programa

        NoArvore<Dado> novoNo = new NoArvore<Dado>(dadoLido); //Criaremos o nó que será inserido

        if (raiz == null) //Caso não haja nós
            raiz = novoNo; //O novo nó se tornará um raiz
        else
        {
            //Verificaremos qual lado do antecessor o nó deve ser colocado e assim faremos
            if (dadoLido.CompareTo(antecessor.Info) < 0)
                antecessor.Esq = novoNo;
            else
                antecessor.Dir = novoNo;
        }

        nos++; //Mostraremos que a quantidade de nós aumentou
    }

    public void DesenharArvore(Graphics g, int largura)
    {
        DesenharArvore(raiz, largura/2 , 0, Math.PI / 2, Math.PI / 2.5, 300, g);
    }

    private void DesenharArvore(NoArvore<Dado> atual,
                               int x, int y, double angulo, double incremento,
                               double comprimento, Graphics g)
    {
        int xf, yf;

        if (atual != null)
        {
            Pen caneta = new Pen(Color.DarkGray);
            xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);

            if (y != 0)
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
            else
                yf = 30;

            g.DrawLine(caneta, x, y, xf, yf);
            DesenharArvore(atual.Esq, xf, yf, Math.PI / 2 + incremento,
                                             incremento * 0.60, comprimento * 0.7, g);

            DesenharArvore(atual.Dir, xf, yf, Math.PI / 2 - incremento,
                                              incremento * 0.60, comprimento * 0.7, g);

            SolidBrush preenchimento = new SolidBrush(Color.BlueViolet);
            g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
            g.DrawString((atual.Info.ParaArvore()), new Font("Courier New", 10),
                          new SolidBrush(Color.Black), xf - atual.Info.ParaArvore().Length * 10 / 4, yf - 7);
        }
    }

    public void ExecutaEmTodos(Action<Dado> metodo)
    {
        ExecutaEmTodos(raiz, metodo); //Executaremos um dado método em todos os nós
    }

    private void ExecutaEmTodos(NoArvore<Dado> atual, Action<Dado> metodo)
    {
        if (atual != null) //Verificaremos se estamos trabalhando com um nó de fato
        {
            //Executaremos o método de modo ordenado
            ExecutaEmTodos(atual.Esq, metodo);
            metodo(atual.Info);
            ExecutaEmTodos(atual.Dir, metodo);
        }
    }
}