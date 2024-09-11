using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using CRUDINMVC.Models;

public class ProductRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection Connection
    {
        get { return new SqlConnection(_connectionString); }
    }

    public IEnumerable<Product> GetAll()
    {
        using (IDbConnection dbConnection = Connection)
        {
            string sql = "SELECT * FROM Products";
            return dbConnection.Query<Product>(sql).ToList();
        }
    }

    public Product GetById(int id)
    {
        using (IDbConnection dbConnection = Connection)
        {
            string sql = "SELECT * FROM Products WHERE Id = @Id";
            return dbConnection.QuerySingleOrDefault<Product>(sql, new { Id = id });
        }
    }

    public void Add(Product product)
    {
        using (IDbConnection dbConnection = Connection)
        {
            string sql = "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";
            dbConnection.Execute(sql, product);
        }
    }

    public void Update(Product product)
    {
        using (IDbConnection dbConnection = Connection)
        {
            string sql = "UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity WHERE Id = @Id";
            dbConnection.Execute(sql, product);
        }
    }

    public void Delete(int id)
    {
        using (IDbConnection dbConnection = Connection)
        {
            string sql = "DELETE FROM Products WHERE Id = @Id";
            dbConnection.Execute(sql, new { Id = id });
        }
    }
}
