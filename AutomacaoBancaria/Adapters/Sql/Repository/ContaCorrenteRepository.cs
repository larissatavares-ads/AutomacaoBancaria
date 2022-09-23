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
    public async Task<ContaCorrente> ConsultarAgencia(int agencia)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            var consultaAgencia =
                (await conexao.QueryAsync<ContaCorrente>(
                    $"SELECT * FROM AgenciaTab WHERE AgenciaNum='{agencia}';")).FirstOrDefault();
            return consultaAgencia;
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
    public async Task<ContaCorrente> EfetivarDeposito(int agencia, int conta, decimal novoSaldo)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            var efetivarDeposito =
                (await conexao.QueryAsync<ContaCorrente>(
                    $"UPDATE ContaCorrente SET Saldo='{novoSaldo}' WHERE Agencia='{agencia}' AND Conta='{conta}';" +
                    $"SELECT * FROM ContaCorrente;")).FirstOrDefault();
            return efetivarDeposito;
        }
    }
    public async Task EfetivarSaque(int agencia, int conta, decimal novoSaldo)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            await conexao.QueryAsync<ContaCorrente>(
                    $"UPDATE ContaCorrente SET Saldo='{novoSaldo}' WHERE Agencia='{agencia}' AND Conta='{conta}';");
        }
    }
    public async Task LogCredito(Log log)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            await conexao
                .ExecuteAsync
                ($"INSERT INTO LogTransacao (DataLog,CodigoLog,AgenciaLog,ContaLog,ValorLog) " +
                 $"VALUES ((SELECT GETDATE()), @CodigoLog, @AgenciaLog, @ContaLog, @ValorLog);", log);
        }
    }
    public async Task LogDebito(Log log)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            await conexao
                .ExecuteAsync
                ($"INSERT INTO LogTransacao (DataLog,CodigoLog,AgenciaLog,ContaLog,ValorLog) " +
                 $"VALUES ((SELECT GETDATE()), @CodigoLog, @AgenciaLog, @ContaLog, @ValorLog);", log);
        }
    }
}