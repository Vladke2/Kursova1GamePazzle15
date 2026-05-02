using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; 

namespace Puzzle15
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int BestMoves { get; set; }
        public int BestTime { get; set; }

        public bool HasSavedGame { get; set; }
        public int SavedSize { get; set; }
        public int SavedMoves { get; set; }
        public int SavedTime { get; set; }
        public List<int> SavedBoard { get; set; } = new List<int>();

        public Dictionary<string, int> WinsPerSize { get; set; } = new Dictionary<string, int>();

        public User() { }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
            BestMoves = 0;
            BestTime = 0;
            HasSavedGame = false;
        }
    }

    public static class UserManager
    {
        // Змінюємо розширення файлу на .json
        private static readonly string filePath = "users.json";

        public static User CurrentUser { get; private set; }
        public static List<User> AllUsers { get; private set; } = new List<User>();
        public static bool IsSpeedrunMode { get; set; } = false;

        public static void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    // Читаємо весь текст і десеріалізуємо з JSON
                    string jsonString = File.ReadAllText(filePath);
                    AllUsers = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
                }
                catch
                {
                    // Якщо файл пошкоджено, починаємо з чистого аркуша
                    AllUsers = new List<User>();
                }
            }
            else
            {
                AllUsers = new List<User>();
            }
        }

        public static void SaveUsers()
        {
            // Налаштовуємо WriteIndented = true, щоб JSON був красивим і читабельним у блокноті
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(AllUsers, options);

            // Записуємо у файл
            File.WriteAllText(filePath, jsonString);
        }

        public static bool Login(string name, string password)
        {
            LoadUsers();
            User foundUser = AllUsers.Find(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (foundUser != null && foundUser.Password == password)
            {
                CurrentUser = foundUser;
                return true;
            }
            return false;
        }

        public static bool Register(string name, string password)
        {
            LoadUsers();
            if (AllUsers.Exists(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase))) return false;
            CurrentUser = new User(name, password);
            AllUsers.Add(CurrentUser);
            SaveUsers();
            return true;
        }

        public static void Logout() { CurrentUser = null; }
    }
}