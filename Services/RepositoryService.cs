using System.Data;

using Dapper;

using Thumbnail.API.Interfaces;

namespace Thumbnail.API.Services
{
    public class RepositoryService: IRepositoryService
    {
        private readonly IDbConnection _dbConnection;

        public RepositoryService(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        public IEnumerable<string> GetFileNames(long offset, long chunkSize)
        {
            string sql = $"SELECT [FILE_File_Name] FROM [dbo].[FLS_Files] ORDER BY [FILE_Id] ASC OFFSET {offset} ROWS FETCH NEXT {chunkSize} ROWS ONLY;";

            return _dbConnection.Query<string>(sql);
        }

        public string GetFileName(string fileId)
        {
            string sql = $"SELECT [FILE_File_Name] FROM [dbo].[FLS_Files] WHERE [FILE_Id] = {fileId};";

            return _dbConnection.QueryFirst<string>(sql);
        }

        public long GetTotalFileCount() 
        {
            string sql = $"SELECT COUNT(*) FROM [dbo].[FLS_Files];";

            return _dbConnection.QueryFirstOrDefault<long>(sql);
        }
    }
}
