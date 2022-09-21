namespace AutomacaoBancaria.Domain.Core.Models;

public class ContaCorrente
{
    public int Conta { get; set; }
    public int Agencia { get; set; }
    public decimal Saldo { get; set; }
    public string CpfTitular { get; set; }
    
    public Titular Titular { get; set; }


    public void Creditar(decimal valor)
    {
        Saldo += valor;
    }

    public void Debitar(decimal valor, ContaCorrente contaDestino)
    {
        if(valor < Saldo)
            throw new ArgumentException("Seu saldo é insuficiente para transferencia", nameof(valor));
        Saldo -= valor;
        contaDestino.Creditar(valor);
    }
}