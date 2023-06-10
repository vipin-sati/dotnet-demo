using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Dapper;
using test_api.Dapper;
using test_api.Interfaces;
using test_api.Interfaces.Repositories;
using test_api.Model;
using static System.Collections.Specialized.BitVector32;

namespace test_api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;
        public ProductRepository(DapperContext context)
        {
            this._context = context;
        }

        public static void SetSqlMap()
        {
            SqlMapper.SetTypeMap(typeof(Product),
                new CustomPropertyTypeMap(typeof(Product),
                                            (type, columnName) => type.GetProperties().FirstOrDefault
                                            (prop => prop.GetCustomAttributes(false).
                                                OfType<ColumnAttribute>().
                                                Any(attr => attr.Name == columnName)
                                            )
                                         )
                                );
        }
        public int AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProduct(int productId)
        {
            var query = "DELETE FROM production.product WHERE ProductID = @productId";
            using (IDbConnection connection = this._context.GetConnection())
            {
                await connection.ExecuteAsync(query, new { productId });
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = new List<Product>();
            var SqlQuery = @"select * from production.product";

            SetSqlMap();

            using (IDbConnection connection = this._context.GetConnection())
            {
                var result = await connection.QueryAsync<Product>(SqlQuery);
                return result.ToList();
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            var query = "SELECT * FROM production.product WHERE ProductID = @productId";
            SetSqlMap();
            using (IDbConnection connection = this._context.GetConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { productId });
                return product;
            }
        }

        public async Task<Int32> UpdateProduct(int productId, Product product)
        {
            int numberOfRows = 0;
            var query = "UPDATE production.product SET Name = @Name, ProductNumber = @ProductNumber, StandardCost = @StandardCost, ListPrice = @ListPrice WHERE ProductID = @ProductID";
            var parameters = new DynamicParameters();
            parameters.Add("ProductID", productId, DbType.Int32);
            parameters.Add("Name", product.Name, DbType.String);
            parameters.Add("ProductNumber", product.ProductNumber, DbType.String);
            parameters.Add("StandardCost", product.StandardCost, DbType.Decimal);
            parameters.Add("ListPrice", product.ListPrice, DbType.Decimal);
            using (IDbConnection connection = this._context.GetConnection())
            {
                numberOfRows = await connection.ExecuteAsync(query, parameters);
            }

            return numberOfRows;
        }
    }
}
