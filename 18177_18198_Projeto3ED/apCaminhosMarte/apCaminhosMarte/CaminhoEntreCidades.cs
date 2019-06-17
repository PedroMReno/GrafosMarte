//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class CaminhoEntreCidades : IComparable<CaminhoEntreCidades>
{
    // declaracao de variaveis que caracterizam o caminho entre cidades
    int origem, destino, distancia, tempo, custo;

    //declaracao de variaveis constantes para facilitar a leitura de arquivo
    const int tamanhoOrigem = 3;
    const int tamanhoDestino = 3;
    const int tamanhoDistancia = 5;
    const int tamanhoTempo = 4;
    const int tamanhoCusto = 5;

    const int inicioDestino = tamanhoOrigem;
    const int inicioDistancia = inicioDestino + tamanhoDestino;
    const int inicioTempo = inicioDistancia + tamanhoDistancia;
    const int inicioCusto = inicioTempo + tamanhoTempo;

    public int Origem
    {
        get => origem;
        set
        {
            if (value < 0) // caso identificacao de origem seja negativa lanca excessao
                throw new Exception("Identificação de origem inválida");
            origem = value;
        }
    }
    public int Destino
    {
        get => destino;
        set
        {
            if (value < 0) // caso identificacao de destino seja negativo lanca excessao
                throw new Exception("Identificação de destino inválida");
            destino = value;
        }
    }
    public int Distancia
    {
        get => distancia;
        set
        {
            if (distancia < 0) // caso distancia seja negativa lanca excessao
                throw new Exception("Distância inválida");
            distancia = value;
        }
    }
    public int Tempo
    {
        get => tempo;
        set
        {
            if (value < 0) // caso tempo seja negativo lanca excessao
                throw new Exception("Tempo de viagem inválido");
            tempo = value;
        }
    }
    public int Custo
    {
        get => custo;
        set
        {
            if (value < 0) // caso custo seja negativo lanca excessao
                throw new Exception("Custo de viagem inválido");
            custo = value;
        }
    }

    public CaminhoEntreCidades(string linha) // construtor que recebe uma linha como parametro 
    {
        // leitura e quebra da linha para armazenamento dos dados referencia do caminho
        Origem = Convert.ToInt32(linha.Substring(0, tamanhoOrigem));
        Destino = Convert.ToInt32(linha.Substring(inicioDestino, tamanhoDestino));
        Distancia = Convert.ToInt32(linha.Substring(inicioDistancia, tamanhoDistancia));
        Tempo = Convert.ToInt32(linha.Substring(inicioTempo, tamanhoTempo));
        Custo = Convert.ToInt32(linha.Substring(inicioCusto, tamanhoCusto));
    }

    public int CompareTo(CaminhoEntreCidades other) // comparacao de caminhos a partir de suas origens
    {
        return origem.CompareTo(other.origem);
    }
}
