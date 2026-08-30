using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);

            _conn.CreateTableAsync<Produto>().Wait();
        }

        // INSERIR PRODUTO
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // ATUALIZAR PRODUTO
        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }

        // EXCLUIR PRODUTO
        // O Id da Model é utilizado como código do produto.
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>()
                        .DeleteAsync(i => i.Id == id);
        }


        // BUSCAR TODOS OS PRODUTOS
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>()
                        .OrderBy(p => p.Descricao)
                        .ToListAsync();
        }

  
        // PESQUISAR PRODUTOS
        public Task<List<Produto>> Search(string q)
        {
            string sql =
                "SELECT * FROM Produto WHERE Descricao LIKE ?";

            return _conn.QueryAsync<Produto>(
                sql,
                "%" + q + "%"
            );
        }
    }
}