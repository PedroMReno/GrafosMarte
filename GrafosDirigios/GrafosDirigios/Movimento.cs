using System;

class Movimento : IComparable<Movimento>
{
    private int cidade;
    private int saida;
    private int valor;

    public Movimento(int cidade, int saida, int valor)
    {
        this.cidade = cidade;
        this.saida = saida;
        this.valor = valor;
    }

    public int Cidade { get => cidade; }
    public int Saida { get => saida; }
    public int Valor { get => valor; }

    public override string ToString()
    {
        return "Cidade " + cidade;
    }

    public int CompareTo(Movimento other)
    {
        throw new NotImplementedException();
    }
}
