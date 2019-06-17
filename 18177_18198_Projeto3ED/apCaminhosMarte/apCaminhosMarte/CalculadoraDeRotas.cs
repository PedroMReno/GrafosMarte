//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CalculadoraDeRotas
{
    private int qtdCidades;

    CaminhoEntreCidades[,] cidades;

    public CalculadoraDeRotas(int qtdCidades, CaminhoEntreCidades[,] cidades)   //construtor da classe
    {
        this.qtdCidades = qtdCidades;
        this.cidades = cidades;
    }

    public List<List<CaminhoEntreCidades>> Calcular(int origem, int destino)
    {
        PilhaLista<CaminhoEntreCidades> pilha = new PilhaLista<CaminhoEntreCidades>(); //Serivirá para fazer o Backtracking
        List<List<CaminhoEntreCidades>> ret = new List<List<CaminhoEntreCidades>>(); //Armazenará todos os caminhos que encontramos
        bool[] passouCidade = new bool[qtdCidades]; //Armazenaremos por quais cidades passamos
        int cidadeAtual = origem;
        int saidaAtual = 0; //Indica qual cidade tentaremos seguir

        while (!(cidadeAtual == origem && saidaAtual == qtdCidades)) //Enquanto não tentamos todas as possibilidades da cidade inicial
        {
            if (saidaAtual < qtdCidades && cidadeAtual != destino) //Apenas continuaremos andando caso ainda hajam cidades a tentar e a
                                                                   //cidade que estamos não é o destino
            {
                for (; saidaAtual < qtdCidades; saidaAtual++) //Tentaremos nos mover para todas as cidades
                    if (passouCidade[saidaAtual] == false && cidades[saidaAtual, cidadeAtual] != null)
                        break; //Caso não passamos pela cidade sendo testada e caso exista uma conecção entre as cidades,
                               //podemos sair do for

                if (saidaAtual != qtdCidades) //Verificaremos se já tentamos todas as cidades, ou seja, se achamos algum caminho possível
                {
                    //Se achamos algum caminho
                    passouCidade[cidadeAtual] = true; //Marcaremos que passados pela determinada cidade
                    pilha.Empilhar(cidades[saidaAtual, cidadeAtual]); //Guardaremos o movimento que fizemos
                    cidadeAtual = saidaAtual; //Avançaremos a cidade
                    saidaAtual = 0; //Prepararemos a variável para o próximo ciclo
                }
            }
            else
            {
                if (cidadeAtual == destino) //Caso achamos o destino
                {
                    /*
                     Iremos guardar o caminho feito até então, para isso, devemos esvaziar a pilha.
                     Porém, existe a chance de ainda precisarmos usar a pilha nesse método, ou seja,
                      a pilha deve continuar igual após lermos seus dados.
                     Por conta disso, devemos colocar os dados da pilha atual numa auxiliar (1), vamos trabalhar
                      com os dados (2) quando os repassarmos para a pilha atual (3).
                     Por fim, forçaremos um backtracking, fazendo com que o algoritmo tente descobrir um novo caminho
                    */

                    PilhaLista<CaminhoEntreCidades> inver = new PilhaLista<CaminhoEntreCidades>();
                    List<CaminhoEntreCidades> nova = new List<CaminhoEntreCidades>();

                    while (!pilha.EstaVazia()) //(1)
                        inver.Empilhar(pilha.Desempilhar());

                    while (!inver.EstaVazia()) //(3)
                    {
                        CaminhoEntreCidades cid = inver.Desempilhar();
                        pilha.Empilhar(cid);
                        nova.Add(cid); //(2)
                    }

                    ret.Add(nova); //Guardaremos o caminho descoberto
                }

                //Fazendo Backtracking
                passouCidade[cidadeAtual] = false; //Desmarcaremos que passamos pela cidade
                CaminhoEntreCidades c = pilha.Desempilhar(); //Recuperaremos o último movimento que fizemos
                cidadeAtual = c.Origem; //Voltaremos a cidade
                saidaAtual = c.Destino + 1; //Evitaremos a possibilidade de acessarmos o mesmo caminho anterior
            }
        }

        return ret;
    }
}
