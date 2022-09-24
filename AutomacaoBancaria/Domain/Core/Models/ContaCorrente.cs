namespace AutomacaoBancaria.Domain.Core.Models;

public class ContaCorrente
{
    public int Conta { get; set; }
    public int Agencia { get; set; }
    public decimal Saldo { get; set; }
    public string CpfTitular { get; set; }
    
    public Titular Titular { get; set; }
    public decimal Creditar(decimal valorDeposito)
    {
        var novoSaldo = Saldo += valorDeposito;
        return novoSaldo;
    }
    public decimal Debitar(decimal valor)
    {
        if(Saldo < valor)
            throw new SaldoInsuficienteException("Seu saldo é insuficiente para saque");
        var novoSaldo = Saldo -= valor;
        return novoSaldo;
    }
}