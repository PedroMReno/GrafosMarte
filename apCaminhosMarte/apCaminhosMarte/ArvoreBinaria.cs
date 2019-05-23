using System;
using System.Drawing;


class ArvoreBinaria<Dado> where Dado : IComparable<Dado> //POde ter menos métodos
{
    private NoArvore<Dado> raiz, atual, antecessor;

    public ArvoreBinaria()
    {
        raiz = atual = antecessor = null;
    }
    public int Altura
    {
        get => AlturaArvore(raiz);
    }
    public bool ExisteDado(Dado procurado)
    {
        antecessor = null;
        atual = raiz;
        while (atual != null)
        {
            if (atual.Info.CompareTo(procurado) == 0)
                return true;
            else
            {
                antecessor = atual;
                if (procurado.CompareTo(atual.Info) < 0)
                    atual = atual.Esq;     // Desloca à esquerda
                else
                    atual = atual.Dir;      // Desloca à direita
            }
        }
        return false;       // Se atual == null, a chave não existe
    }
    public void Incluir(Dado dadoLido)
    {
        if (ExisteDado(dadoLido))
            throw new Exception("Dado já existente");

        NoArvore<Dado> novoNo = new NoArvore<Dado>(dadoLido);
        if (raiz == null)
            raiz = novoNo;
        else
          if (dadoLido.CompareTo(antecessor.Info) < 0)
            antecessor.Esq = novoNo;
        else
            antecessor.Dir = novoNo;
    }
    private int AlturaArvore(NoArvore<Dado> atual) //Perguntar depois sobre balanceada
    {
        int alturaDireita, alturaEsquerda, result;
        if (atual != null)
        {
            alturaDireita = 1 + AlturaArvore(atual.Dir);
            alturaEsquerda = 1 + AlturaArvore(atual.Esq);

            if (alturaDireita > alturaEsquerda)
                result = alturaDireita;
            else
                result = alturaEsquerda;
        }
        else
            result = 0;

        return result;
    }

    public void DesenharArvore(Graphics g, int largura)
    {
        DesenharArvore(raiz, largura/2 , 0, Math.PI / 2, Math.PI / 2.5, 150 ,g);
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
            yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);

            g.DrawLine(caneta, x, y, xf, yf);
            DesenharArvore(atual.Esq, xf, yf, Math.PI / 2 + incremento,
                                             incremento * 0.60, comprimento * 0.8, g);

            DesenharArvore(atual.Dir, xf, yf, Math.PI / 2 - incremento,
                                              incremento * 0.60, comprimento * 0.8, g);

            SolidBrush preenchimento = new SolidBrush(Color.BlueViolet);
            g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
            g.DrawString(Convert.ToString(raiz.Info), new Font("Courier New", 12),
                          new SolidBrush(Color.Black), xf - 15, yf - 10);
        }
    }

    
}