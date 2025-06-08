using System.Data;
using BlackJackGame.Models;
using Dapper;
using MySqlConnector;

namespace HelperLibrary.DataAccess;

public static class MariaDbHelper
{
    private static string _connectionString = "Server=localhost;Database=BlackJackGameDB;Uid=BJUser;Pwd=BJUserPassword123";

    public static void AddMoney(User user, decimal money)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spAddMoney(@amount, @uname)", new
            {
                amount = money, uname = user.Username
            });
        }
    }

    public static void RemoveMoney(User user, decimal money)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spRemoveMoney(@amount, @uname)", new
            {
                amount = money, uname = user.Username
            });       
        }
    }

    public static void AddUser(User user)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spAddUser(@uname, @pass, @em)", new
            {
                uname = user.Username, 
                pass = user.HashedPassword,
                em = user.Email
            });
        }
    }

    public static void RemoveUser(User user)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spRemoveUser(@uname)", new { uname = user.Username });
        }
    }

    public static bool CheckCredentials(string username, string password)
    {
        User? receivedUser;
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            receivedUser = conn.QueryFirstOrDefault<User>("call spCheckCredentials(@uname)", new
            {
                uname = username
            });
        }

        return receivedUser != null && 
               BCrypt.Net.BCrypt.EnhancedVerify(password, receivedUser.HashedPassword);
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

    public static decimal GetBalance(User user)
    {
        decimal balance;
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            balance = conn.QueryFirstOrDefault<decimal>("call spGetBalance(@uname)", new
            {
                uname = user.Username
            });
        }
        return balance;   
    }

    public static void ChangePassword(User user)
    {
        using (IDbConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Execute("call spChangePassword(@uname, @newPass)", new
            {
                uname = user.Username,
                newPass = user.HashedPassword
            });
        }
    }
}