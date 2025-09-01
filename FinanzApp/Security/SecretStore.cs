using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace FinanzApp.Security
{
    public class SecretStore
    {
        #region Fields
        private static string Folder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinanzApp");
        private static string FilePath => Path.Combine(Folder, "secret.bin");

        #endregion

        #region Methods
        public static bool TryLoad(out string password)
        {
            password = "";
            if (!File.Exists(FilePath))
                return false;

            var protectedBytes = File.ReadAllBytes(FilePath);
            var plain = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
            password = Encoding.UTF8.GetString(plain);
            return true;
        }

        public static void Save(string password)
        {
            Directory.CreateDirectory(Folder);
            var plain = Encoding.UTF8.GetBytes(password);
            var protectedBytes = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(FilePath,protectedBytes);

        }

        public static void EnsureDbPassword()
        {
            if (TryLoad(out _)) return;

            // TODO: Ersetze das durch einen eigenen WPF-Dialog mit PasswordBox
            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Bitte DB-Passwort setzen (SQLCipher):", "FinanzApp", "");
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidOperationException("Kein Passwort gesetzt.");

            Save(input);
        }

        #endregion
    }
}
