using System;
using System.Collections.Generic;
using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        const int maxCidade = 11;
        const int cidadeFinal = 3;
        const int cidadeInicial = 6;
        PilhaLista<Movimento> pilha = new PilhaLista<Movimento>();
        List<int> todosAndados = new List<int>();
        bool[] passouCidade = { false, false, false, false, false, false, false, false, false, false, false };

        int saidaAtual = 0;

        while (!(cidadeAtual == cidadeInicial && saidaAtual == maxCidade))
        {
            if(saidaAtual < maxCidade && cidadeAtual != cidadeFinal)
            {
                while (saidaAtual < maxCidade)
                {
                    if (passouCidade[saidaAtual] == false && cidades[cidadeAtual, saidaAtual] > -1)
                        break;

                    saidaAtual++;
                }

                if(saidaAtual != maxCidade)
                {
                    passouCidade[cidadeAtual] = true;
                    pilha.Empilhar(new Movimento(cidadeAtual, saidaAtual, cidades[cidadeAtual, saidaAtual]));
                    cidadeAtual = saidaAtual;
                    saidaAtual = 0;
                }
            }
            else
            {
                if(cidadeAtual == cidadeFinal)
                {
                    PilhaLista<Movimento> inver = new PilhaLista<Movimento>();
                    inver.Empilhar(new Movimento(cidadeAtual, saidaAtual, 0));

                    while (!pilha.EstaVazia())
                        inver.Empilhar(pilha.Desempilhar());

                    int totalAndado = 0;

                    Write("Caminho encontrado: ");

                    while (!inver.EstaVazia())
                    {
                        Movimento mov = inver.Desempilhar();
                        totalAndado += mov.Valor;
                        Write(mov + "; ");
                        pilha.Empilhar(mov);
                    }

                    Write("Total andado: " + totalAndado);
                    WriteLine();

                    todosAndados.Add(totalAndado);
                    pilha.Desempilhar();
                }

                passouCidade[cidadeAtual] = false;
                Movimento m = pilha.Desempilhar();
                cidadeAtual = m.Cidade;
                saidaAtual = m.Saida + 1;
            }
        }

        int menor = 0;
        int maior = 0;

        for(int i = 0; i < todosAndados.Count; i++)
        {
            if (todosAndados[i] < todosAndados[menor])
                menor = i;

            if (todosAndados[i] > todosAndados[maior])
                maior = 1;
        }

        WriteLine("Menor caminho: " + (menor + 1));
        WriteLine("Maior caminho: " + (maior + 1));

        ReadLine();
    }
}
