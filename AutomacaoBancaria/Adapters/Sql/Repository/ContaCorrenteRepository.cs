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
                .ExecuteAsync(
                    "INSERT INTO ContaCorrente (Conta, Agencia, Saldo, CpfTitular) VALUES (@Conta, @Agencia, @Saldo, @CpfTitular);",
                    contaCorrente);

        }
    }
    public async Task<ContaCorrente> ConsultarSaldo(int agencia, int conta)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            var consultaSaldo =
                (await conexao.QueryAsync<ContaCorrente>(
                    $"SELECT * FROM ContaCorrente WHERE Agencia='{agencia}' AND Conta='{conta}';")).FirstOrDefault();
            return consultaSaldo;
        }
    }
    public async Task<ContaCorrente> ConsultarAgencia(int agencia)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            var consultaAgencia =
                (await conexao.QueryAsync<ContaCorrente>(
                    $"SELECT * FROM AgenciaTabela WHERE AgenciaLista='{agencia}';")).FirstOrDefault();
            return consultaAgencia;
        }
    }
}