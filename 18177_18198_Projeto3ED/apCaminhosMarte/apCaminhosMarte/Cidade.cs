//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Cidade : IComparable<Cidade>, IParaArvore
{
    // declaracao das variaveis base de uma cidade
    int id, x, y;
    string nome;

    //declaracao de variaveis constantes para facilitar a leitura de arquivo
    const int tamanhoId = 3;
    const int tamanhoNome = 15;
    const int tamanhoX = 5;
    const int tamanhoY = 5;

    const int inicioNome = 0 + tamanhoId;
    const int inicioX = inicioNome + tamanhoNome;
    const int inicioY = inicioX + tamanhoX;

    public int Id
    {
        get => id;
        set
        {
            if (value < 0) // caso a identificacao seja negativa lanca excessao
                throw new Exception("Id de cidade inválido");

            id = value;
        }
    }
    public string Nome
    {
        get => nome.Trim();
        set
        {
            if (string.IsNullOrWhiteSpace(value)) // caso nome seja nulo ou um espaco vazio lancar excessao
                throw new Exception("Nome de cidade inválido");

            nome = value;
        }
    }
    public int X
    {
        get => x;
        set
        {
            if (value < 0) // caso a cordenada seja negativa lancar excessao
                throw new Exception("Coodenada x de cidade inválida");
            x = value;
        }
    }
    public int Y
    {
        get => y;
        set
        {
            if (value < 0) // caso a cordenada seja negativa lancar excessao
                throw new Exception("Coordenada y de cidade inválida");
            y = value;
        }
    }

    public Cidade(string linha) // construtor que recebe uma linha como parametro
    {
        // leitura e quebra da linha para armazenamento dos dados referencia da cidade
        Id = Convert.ToInt32(linha.Substring(0, tamanhoId)); 
        Nome = linha.Substring(inicioNome, tamanhoNome);
        X = Convert.ToInt32(linha.Substring(inicioX, tamanhoX));
        Y = Convert.ToInt32(linha.Substring(inicioY, tamanhoY));
    }

    public Cidade(int id) // construtor com um so parametro usado para pesquisar entre as cidade pre-existentes
    {
        Id = id;
        nome = "";
        x = 0;
        y = 0;
    }
    
    public int CompareTo(Cidade other) // compararcao de cidades por numero de identificacao
    {
        return id.CompareTo(other.id);
    }

    public override string ToString()
    {
        return Id + "- " + Nome;
    }

    public string ParaArvore() // metodo requerido pela interface que deixa os dados no formato certo para a exibicao em arvore
    {
        int espaco = (Nome.Length / 2) + 1; // 

        return (id.ToString().PadLeft(espaco, ' ') + "\n" + Nome);
    }
}
