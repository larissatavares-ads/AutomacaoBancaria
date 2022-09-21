using System.Data;
using System.Data.SqlClient;
using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Models;
using AutomacaoBancaria.Domain.Core.Settings;
using Dapper;

namespace AutomacaoBancaria.Adapters.Sql.Repository;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private string _connectionString;
    public ContaCorrenteRepository(ConnectionStringSettings settings)
    {
        _connectionString = settings.ConnectionString();
    }
    public async Task CriarContaCorrente(ContaCorrente contaCorrente)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            await conexao
                .ExecuteAsync("INSERT INTO ContaCorrente (Conta, Agencia, Saldo, CpfTitular) VALUES (@Conta, @Agencia, @Saldo, @CpfTitular);", contaCorrente); 

        }
    } 
    public async Task<ContaCorrente> ConsultarExtrato(string cpf)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            var extrato = (await conexao.QueryAsync<ContaCorrente>($"SELECT * FROM ContaCorrente WHERE CpfTitular='{cpf}';")).Single();
            return extrato;
        }
    }
}