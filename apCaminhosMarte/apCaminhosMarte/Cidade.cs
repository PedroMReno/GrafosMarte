using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Cidade : IComparable<Cidade>, IParaArvore
{
    int id, x, y;
    string nome;

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
            if (value < 0)
                throw new Exception("Id de cidade inválido");
            id = value;
        }
    }
    public string Nome
    {
        get => nome.Trim();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Nome de cidade inválido");

            nome = value;
        }
    }
    public int X
    {
        get => x;
        set
        {
            if (value < 0)
                throw new Exception("Coodenada x de cidade inválida");
            x = value;
        }
    }
    public int Y
    {
        get => y;
        set
        {
            if (value < 0)
                throw new Exception("Coordenada y de cidade inválida");
            y = value;
        }
    }

    public Cidade(string linha)
    {
        Id = Convert.ToInt32(linha.Substring(0, tamanhoId));
        Nome = linha.Substring(inicioNome, tamanhoNome);
        X = Convert.ToInt32(linha.Substring(inicioX, tamanhoX));
        Y = Convert.ToInt32(linha.Substring(inicioY, tamanhoY));
    }

    public Cidade(int id)
    {
        Id = id;
        nome = "";
        x = 0;
        y = 0;
    }
    
    public int CompareTo(Cidade other)
    {
        return id.CompareTo(other.id);
    }

    public override string ToString()
    {
        return Nome;
    }

    public string ParaArvore()
    {
        int espaco = (Nome.Length / 2) + 1;

        return (id.ToString().PadLeft(espaco, ' ') + "\n" + Nome);
    }
}
