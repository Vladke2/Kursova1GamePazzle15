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
            { "StatsFmt", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Ходи: {0}   |   Час: {1}" }, { AppLanguage.English, "Moves: {0}   |   Time: {1}" } } },

            { "VicTitle", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Результат" }, { AppLanguage.English, "Result" } } },
            { "VicMsg", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "Вітаємо з перемогою!\n\nЧас гри: {0}\nЗроблено ходів: {1}" }, { AppLanguage.English, "Congratulations! You won!\n\nTime: {0}\nMoves made: {1}" } } },
            { "ContinueBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ПРОДОВЖИТИ" }, { AppLanguage.English, "CONTINUE" } } },
            { "OkBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ОК" }, { AppLanguage.English, "OK" } } },
            { "ToMenuBtn", new Dictionary<AppLanguage, string> { { AppLanguage.Ukrainian, "ГОЛОВНЕ МЕНЮ" }, { AppLanguage.English, "MAIN MENU" } } },   
        };

        public static string Get(string key)
        {
            if (Texts.ContainsKey(key) && Texts[key].ContainsKey(CurrentLanguage)) return Texts[key][CurrentLanguage];
            return key;
        }
    }
}