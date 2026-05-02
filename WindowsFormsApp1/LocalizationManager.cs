using System.Collections.Generic;

namespace Puzzle15
{
    public enum AppLanguage { Ukrainian, English }

    public static class LocalizationManager
    {
        public static AppLanguage CurrentLanguage { get; set; } = AppLanguage.Ukrainian;

        private static readonly Dictionary<string, Dictionary<AppLanguage, string>> Texts = new Dictionary<string, Dictionary<AppLanguage, string>>
        {
            { "MenuTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "П'ятнашки Pro - Меню" }, { AppLanguage.English, "Puzzle 15 Pro - Menu" } } },
            { "InputSize", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Введіть розмір поля (3-10):" }, { AppLanguage.English, "Enter board size (3-10):" } } },
            { "StartGame", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПОЧАТИ НОВУ ГРУ" }, { AppLanguage.English, "START NEW GAME" } } },
            { "ContinueGame", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПРОДОВЖИТИ ГРУ" }, { AppLanguage.English, "CONTINUE GAME" } } }, // НОВИЙ РЯДОК
            { "AuthBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "АВТОРИЗАЦІЯ" }, { AppLanguage.English, "LOG IN" } } },
            { "LogoutBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ВИЙТИ З АКАУНТА" }, { AppLanguage.English, "LOG OUT" } } },
            { "SettingsBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "НАЛАШТУВАННЯ" }, { AppLanguage.English, "SETTINGS" } } },
            { "ProfileGuest", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Профіль: Гість" }, { AppLanguage.English, "Profile: Guest" } } },
            { "Profile", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Профіль: " }, { AppLanguage.English, "Profile: " } } },
            { "AuthPrompt", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Авторизуйтесь, щоб зберігати рекорди" }, { AppLanguage.English, "Log in to save your records" } } },
            { "Record", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Рекорд - Ходи: {0} | Час: {1}" }, { AppLanguage.English, "Record - Moves: {0} | Time: {1}" } } },

            { "SettingsTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Налаштування" }, { AppLanguage.English, "Settings" } } },
            { "LangLabel", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Мова інтерфейсу:" }, { AppLanguage.English, "Interface Language:" } } },
            { "LangToggle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "УКРАЇНСЬКА" }, { AppLanguage.English, "ENGLISH" } } },
            { "CloseBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ЗАКРИТИ" }, { AppLanguage.English, "CLOSE" } } },

            { "AuthTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Авторизація" }, { AppLanguage.English, "Authorization" } } },
            { "NameLbl", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Нікнейм:" }, { AppLanguage.English, "Nickname:" } } },
            { "PassLbl", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Пароль (мін. 4 симв.):" }, { AppLanguage.English, "Password (min. 4 chars):" } } },
            { "LoginBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "УВІЙТИ" }, { AppLanguage.English, "LOG IN" } } },
            { "RegBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "РЕЄСТРАЦІЯ" }, { AppLanguage.English, "REGISTER" } } },
            { "ErrFillFields", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Заповніть всі поля!" }, { AppLanguage.English, "Please fill in all fields!" } } },
            { "ErrPassLen", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Пароль повинен містити\nне менше 4 символів!" }, { AppLanguage.English, "Password must be\nat least 4 characters long!" } } },
            { "ErrLogin", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Невірний нікнейм або пароль!" }, { AppLanguage.English, "Invalid nickname or password!" } } },
            { "ErrRegExists", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Користувач з таким нікнеймом\nвже існує!" }, { AppLanguage.English, "A user with this nickname\nalready exists!" } } },
            { "SuccReg", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Акаунт успішно створено!\nВи увійшли в систему." }, { AppLanguage.English, "Account successfully created!\nYou are logged in." } } },
            { "ErrorTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Помилка" }, { AppLanguage.English, "Error" } } },
            { "SuccessTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Успіх" }, { AppLanguage.English, "Success" } } },

            { "GameTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "П'ятнашки Pro" }, { AppLanguage.English, "Puzzle 15 Pro" } } },
            { "PauseBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПАУЗА" }, { AppLanguage.English, "PAUSE" } } },
            { "ResumeBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПРОДОВЖИТИ" }, { AppLanguage.English, "RESUME" } } },
            { "PauseOverlay", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ГРА НА ПАУЗІ" }, { AppLanguage.English, "GAME PAUSED" } } },
            { "StatsFmt", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Ходи: {0} | Час: {1}" }, { AppLanguage.English, "Moves: {0} | Time: {1}" } } },

            { "VicTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Результат" }, { AppLanguage.English, "Result" } } },
            { "VicMsg", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Вітаємо з перемогою!\n\nЧас гри: {0}\nЗроблено ходів: {1}" }, { AppLanguage.English, "Congratulations! You won!\n\nTime: {0}\nMoves made: {1}" } } },
            { "ContinueBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПРОДОВЖИТИ" }, { AppLanguage.English, "CONTINUE" } } },
            { "OkBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ОК" }, { AppLanguage.English, "OK" } } },
            { "ToMenuBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ГОЛОВНЕ МЕНЮ" }, { AppLanguage.English, "MAIN MENU" } } },
            { "AutoSolveBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "АВТОРОЗВ'ЯЗОК" }, { AppLanguage.English, "AUTO SOLVE" } } },
            { "NoHistory", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Немає збереженої історії ходів\nдля цієї партії!" }, { AppLanguage.English, "No move history available\nfor this session!" } } },
            { "AutoSolveComplete", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Збірка завершена успішно!" }, { AppLanguage.English, "Solving completed successfully!" } } },
            { "SpeedrunLabel", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Спідран (хід наведенням):" }, { AppLanguage.English, "Speedrun (hover to move):" } } },
            { "On", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "УВІМК" }, { AppLanguage.English, "ON" } } },
            { "Off", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ВИМК" }, { AppLanguage.English, "OFF" } } },

            { "StatsBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "СТАТИСТИКА" }, { AppLanguage.English, "STATISTICS" } } },
            { "StatsFormTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Статистика" }, { AppLanguage.English, "Statistics" } } },
            { "TabPersonal", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ОСОБИСТА" }, { AppLanguage.English, "PERSONAL" } } },
            { "TabGlobal", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ГЛОБАЛЬНА" }, { AppLanguage.English, "GLOBAL" } } },
            { "NoStats", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Ще немає статистики." }, { AppLanguage.English, "No statistics yet." } } },
            { "PersonalStatsText", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Гравець: {0}\n\nНайкращий час (загалом): {1}\nМінімум ходів (загалом): {2}\n\nКількість перемог:\n" }, { AppLanguage.English, "Player: {0}\n\nBest Time (Overall): {1}\nBest Moves (Overall): {2}\n\nTotal Wins:\n" } } },
            { "GlobalStatsText", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ТОП-5 ГРАВЦІВ (Найкращий час)\n\n" }, { AppLanguage.English, "TOP-5 PLAYERS (Best Time) \n\n" } } },
        };

        public static string Get(string key)
        {
            if (Texts.ContainsKey(key) && Texts[key].ContainsKey(CurrentLanguage)) return Texts[key][CurrentLanguage];
            return key;
        }
    }
}