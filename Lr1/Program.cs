namespace Lr1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        ///  39 символов (јЕZ, пробел, Ђ.ї, Ђ,ї, цифры 0Е9)
        ///  4)	система ÷езар€ с ключевым словом;
        ///  8)	простые шифрующие таблицы;
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }
    }
}