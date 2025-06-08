using System.Data;
using Dapper;
using MySqlConnector;

namespace HelperLibrary.DataAccess;

public static class MariaDbHelper
{
    private static string _connectionString = "Server=localhost;Database=BlackJackGameDB;Uid=BJUser;Pwd=BJUserPassword123";

    public static void AddMoney(string username, decimal money)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spAddMoney(@amount, @uname)", new
            {
                amount = money, uname = username
            });
        }
    }

    public static void RemoveMoney(string username, decimal money)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spRemoveMoney(@amount, @uname)", new
            {
                amount = money, uname = username
            });       
        }
    }

    public static void AddUser(string username, string hashedPassword, string email)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spAddUser(@uname, @pass, @em)", new
            {
                uname = username,
                pass = hashedPassword,
                em = email
            });
        }
    }

    public static void RemoveUser(string username)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spRemoveUser(@uname)", new { uname = username });
        }
    }

    public static bool CheckCredentials(string username, string password)
    {
        dynamic? receivedUser;
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            receivedUser = conn.QueryFirstOrDefault("call spCheckCredentials(@uname)", new
            {
                uname = username
            });
        }

        return receivedUser != null && 
               BCrypt.Net.BCrypt.EnhancedVerify(password, receivedUser?.HashedPassword);
    }

    public static bool CheckUserAlreadyExists(string username, string email)
    {
        int result;
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            result = conn.ExecuteScalar<int>("call spCheckUserRepeats(@uname, @em)", new
            {
                uname = username,
                em = email
            });
        }
        return result > 0;
    }

    public static decimal GetBalance(string username)
    {
        decimal balance;
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            balance = conn.QueryFirstOrDefault<decimal>("call spGetBalance(@uname)", new
            {
                uname = username
            });
        }
        return balance;   
    }

    public static void ChangePassword(string username, string newHashedPassword)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spChangePassword(@uname, @newPass)", new
            {
                uname = username,
                newPass = newHashedPassword
            });
        }
    }
}