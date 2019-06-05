using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CalculadoraDeRotas
{
    private int qtdCidades;

    List<int> todosAndados = new List<int>(); //

    CaminhoEntreCidades[,] cidades;

    //construtor da classe
    public CalculadoraDeRotas(int qtdCidades, CaminhoEntreCidades[,] cidades)
    {
        this.qtdCidades = qtdCidades;
        this.cidades = cidades;
    }

    public void Calcular(int origem, int destino)
    {
        int cidadeAtual = origem;
        int saidaAtual = 0;

        bool[] passouCidade = { false, false, false, false, false, false, false, false, false, false, false }; // mudar

        PilhaLista<CaminhoEntreCidades> pilha = new PilhaLista<CaminhoEntreCidades>();

        while (!(cidadeAtual == origem && saidaAtual == qtdCidades))
        {
            if (saidaAtual < qtdCidades && cidadeAtual != destino)
            {
                while (saidaAtual < qtdCidades)
                {
                    if (passouCidade[saidaAtual] == false && cidades[cidadeAtual, saidaAtual] != null)
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
                    inver.Empilhar(cidades[saidaAtual, cidadeAtual]);

                    while (!pilha.EstaVazia())
                        inver.Empilhar(pilha.Desempilhar());

                    int totalAndado = 0;

                    //Write("Caminho encontrado: ");

                    while (!inver.EstaVazia())
                    {
                        CaminhoEntreCidades cid = inver.Desempilhar();
                        //totalAndado += cid.Valor;
                        //Write(cid + "; ");
                        pilha.Empilhar(cid);
                    }

                    //Write("Total andado: " + totalAndado);
                    //WriteLine();

                    todosAndados.Add(totalAndado);
                    pilha.Desempilhar();
                }

                passouCidade[cidadeAtual] = false;
                CaminhoEntreCidades c = pilha.Desempilhar();
                cidadeAtual = c.Origem;
                saidaAtual = c.Destino + 1;
            }
        }

        int menor = 0;
        int maior = 0;

        for (int i = 0; i < todosAndados.Count; i++)
        {
            if (todosAndados[i] < todosAndados[menor])
                menor = i;

            if (todosAndados[i] > todosAndados[maior])
                maior = 1;
        }

        //WriteLine("Menor caminho: " + (menor + 1));
        //WriteLine("Maior caminho: " + (maior + 1));

        //ReadLine();
    }
}
