using System.Diagnostics;
using UnityEngine;

/// Clase encargada de usar la linea de comandos para abrir la app 
public class CommandLineHandler : MonoBehaviour
{
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        // Comprueba si se proporcionaron argumentos de línea de comando
        if (args != null && args.Length > 1)
        {
            // Obtén el primer argumento (el segundo elemento del array de argumentos)
            string command = args[1];

            // Verifica el comando y realiza la acción correspondiente
            if (command == "openTextToolApp")
            {
                // Realiza la acción deseada, como abrir la aplicación
                OpenApplication();
            }
        }
    }

    void OpenApplication()
    {        
        string filePath = Application.dataPath + "/TextTranslatorTool.exe";

        // Ejecuta el archivo
        Process.Start(filePath);
    }
}
