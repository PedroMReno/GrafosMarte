using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CalculadoraDeRotas
{
    private int qtdCidades;

    CaminhoEntreCidades[,] cidades;

    //construtor da classe
    public CalculadoraDeRotas(int qtdCidades, CaminhoEntreCidades[,] cidades)
    {
        this.qtdCidades = qtdCidades;
        this.cidades = cidades;
    }

    public List<List<CaminhoEntreCidades>> Calcular(int origem, int destino)
    {
        PilhaLista<CaminhoEntreCidades> pilha = new PilhaLista<CaminhoEntreCidades>();
        List<List<CaminhoEntreCidades>> ret = new List<List<CaminhoEntreCidades>>();
        bool[] passouCidade = new bool[qtdCidades];
        int cidadeAtual = origem;
        int saidaAtual = 0;

        while (!(cidadeAtual == origem && saidaAtual == qtdCidades))
        {
            if (saidaAtual < qtdCidades && cidadeAtual != destino)
            {
                while (saidaAtual < qtdCidades)
                {
                    if (passouCidade[saidaAtual] == false && cidades[saidaAtual, cidadeAtual] != null)
                        break;

                    saidaAtual++;
                }

                if (saidaAtual != qtdCidades)
                {
                    passouCidade[cidadeAtual] = true;
                    pilha.Empilhar(cidades[saidaAtual, cidadeAtual]);
                    cidadeAtual = saidaAtual;
                    saidaAtual = 0;
                }
            }
            else
            {
                if (cidadeAtual == destino)
                {
                    PilhaLista<CaminhoEntreCidades> inver = new PilhaLista<CaminhoEntreCidades>();
                    List<CaminhoEntreCidades> nova = new List<CaminhoEntreCidades>();

                    while (!pilha.EstaVazia())
                        inver.Empilhar(pilha.Desempilhar());

                    while (!inver.EstaVazia())
                    {
                        CaminhoEntreCidades cid = inver.Desempilhar();
                        pilha.Empilhar(cid);
                        nova.Add(cid);
                    }

                    ret.Add(nova);
                }

                passouCidade[cidadeAtual] = false;
                CaminhoEntreCidades c = pilha.Desempilhar();
                cidadeAtual = c.Origem;
                saidaAtual = c.Destino + 1;
            }
        }

        return ret;
    }
}
