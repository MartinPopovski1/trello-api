using Projector.Core.Logic.Handlers.ProjectHandler;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories.RowRecordParsers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public class ProjectRepository : IProjectRepository
    {
        private string _connString;
        public ProjectRepository(string connectionString)
        {
            _connString = connectionString;
        }
        public async Task<Project> AddProject(Project p)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $"INSERT INTO Project(Id,Name,CreatedAt) VALUES ('{p.Id}','{p.Name}','{p.CreatedAt}')";

                await command.ExecuteNonQueryAsync();

                connection.Close();
                return p;
            }
        }

        public async Task<List<Project>> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                string sqlCommand = "SELECT p.Id as ProjectId, p.Name as ProjectName, p.CreatedAt as ProjectCreatedAt FROM Project p Where p.Deleted <> 1 ORDER BY ProjectCreatedAt DESC";

                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                List<Project> projects = new List<Project>();

                while (rowRecords.Read())
                {

                    var project = RowRecordParser.ParseProject(rowRecords);
                    projects.Add(project);

                }

                connection.Close();
                return projects;
            }
        }

        public async Task<List<Project>> Search(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"SELECT p.Id as ProjectId, p.Name as ProjectName, p.CreatedAt as ProjectCreatedAt 
                                        FROM Project p 
                                        WHERE p.Name LIKE '%{name}%' AND p.Deleted != 1";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                List<Project> projects = new List<Project>();

                while (rowRecords.Read())
                {

                    var project = RowRecordParser.ParseProject(rowRecords);
                    projects.Add(project);

                }
                connection.Close();
                return projects;
            }


        }


        public async Task<Project> Get(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();
                string sqlCommand;

                sqlCommand = $@"SELECT p.Id as ProjectId, p.Name as ProjectName, p.CreatedAt as ProjectCreatedAt, b.Id as BoardId, b.Name as BoardName, b.CreatedAt as BoardCreatedAt 
                                FROM Project p 
                                LEFT JOIN (SELECT * FROM Board WHERE Deleted != 1) AS b on b.ProjectId = p.Id 
                                WHERE p.Deleted != 1 AND p.Id = '{id}'
                                ORDER BY BoardCreatedAt DESC";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                var tempId = Guid.NewGuid();
                Project project = new Project();

                while (rowRecords.Read())
                {
                    if (Guid.Parse(rowRecords["ProjectId"].ToString()) != tempId)
                    {
                        project = RowRecordParser.ParseProject(rowRecords);
                        tempId = project.Id;
                    }
                    if (string.IsNullOrEmpty(rowRecords["BoardName"].ToString()))
                        continue;
                    var board = RowRecordParser.ParseBoard(rowRecords);

                    project.Boards.Add(board);
                }

                connection.Close();
                return project;
            }
        
        }
        public async Task Delete(Guid projectId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $"UPDATE Project SET Deleted = 1 Where Id = '{projectId}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task Edit(Guid projectId, string Name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $"UPDATE Project SET Name = '{Name}' WHERE Id = '{projectId}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();



                connection.Close();



            }
        }

        public async Task<bool> NameTaken(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $"SELECT Name FROM Project p WHERE p.Name = '{name}' AND p.Deleted != 1";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                var rowRecords = await command.ExecuteReaderAsync();
                if (rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;

            }
        }

        public async Task<bool> HasActiveBoards(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $"SELECT Name FROM Board b WHERE b.ProjectId = '{id}' AND b.Deleted != 1";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                var rowRecords = await command.ExecuteReaderAsync();

                if (rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }

                connection.Close();
                return false;

            }
        }
    }


}