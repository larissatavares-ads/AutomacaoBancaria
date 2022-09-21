namespace AutomacaoBancaria.Domain.Core.Settings;

public class ConnectionStringSettings
{
    public string IP { get; set; }
    public string Banco { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }
    public string Porta { get; set; }

    public string ConnectionString()
    {
        return $"Server={IP},{Porta};Database={Banco};User Id={Usuario};Password={Senha};";
    }
}