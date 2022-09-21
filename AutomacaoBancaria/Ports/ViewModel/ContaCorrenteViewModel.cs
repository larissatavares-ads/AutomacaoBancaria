using AutomacaoBancaria.Domain.Core.Models;

namespace AutomacaoBancaria.Ports.ViewModel;

public class ContaCorrenteViewModel
{
    public int Conta { get; set; }
    public int Agencia { get; set; }
    public decimal Saldo { get; set; }
    public string CpfTitular { get; set; }
}