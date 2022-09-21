using System.Data;
using System.Data.SqlClient;
using AutomacaoBancaria.Domain.Core.Interfaces.Adapters.Sql;
using AutomacaoBancaria.Domain.Core.Models;
using AutomacaoBancaria.Domain.Core.Settings;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AutomacaoBancaria.Adapters.Sql.Repository;

public class TitularRepository : ITitularRepository
{
    private string _connectionString;
    public TitularRepository(ConnectionStringSettings settings)
    {
        _connectionString = settings.ConnectionString();
    }

    public async Task CriarTitular(Titular titular)
    {
        using (IDbConnection conexao = new SqlConnection(_connectionString))
        {
            conexao.Open();
            await conexao
                .ExecuteAsync("INSERT INTO Titular (Nome, Cpf) VALUES (@Nome, @Cpf);", titular); 
        }
    } 
}