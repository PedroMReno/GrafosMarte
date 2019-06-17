//Amabile Pietrobon Ferreira - 18198
//Pedro Henrique Marques Renó - 18177

using System;

public interface IStack<Dado> where Dado : IComparable<Dado>
{
  void Empilhar(Dado elemento);
  Dado Desempilhar();
  Dado OTopo();
  bool EstaVazia();
  int Tamanho();
}
