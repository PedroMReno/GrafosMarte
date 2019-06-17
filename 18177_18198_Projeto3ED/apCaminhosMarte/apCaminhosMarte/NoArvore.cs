//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;

class NoArvore<Tipo> : IComparable<NoArvore<Tipo>>
    where Tipo : IComparable<Tipo>
{
    private Tipo info;
    private NoArvore<Tipo> esq;
    private NoArvore<Tipo> dir;
    private int altura;
    public NoArvore(Tipo info)
    {
        this.info = info;
        this.esq = null;
        this.dir = null;
        altura = 0;
    }
    public NoArvore(Tipo info, NoArvore<Tipo> esquerdo, NoArvore<Tipo> direito,
            int altura)
    {
        this.info = info;
        this.esq = esquerdo;
        this.dir = direito;
        this.altura = altura;
    }

    public Tipo Info { get => info; set => info = value; }
    public int Altura { get => altura; set => altura = value; }
    internal NoArvore<Tipo> Esq { get => esq; set => esq = value; }
    internal NoArvore<Tipo> Dir { get => dir; set => dir = value; }

    public int CompareTo(NoArvore<Tipo> o)
    {
        return info.CompareTo(o.info);
    }
    public bool Equals(NoArvore<Tipo> o)
    {
        return this.info.Equals(o.info);
    }
}