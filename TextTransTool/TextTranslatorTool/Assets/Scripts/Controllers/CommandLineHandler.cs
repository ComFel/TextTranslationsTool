using System.Diagnostics;
using UnityEngine;

/// Clase encargada de usar la linea de comandos para abrir la app 
public class CommandLineHandler : MonoBehaviour
{
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        // Comprueba si se proporcionaron argumentos de l�nea de comando
        if (args != null && args.Length > 1)
        {
            // Obt�n el primer argumento (el segundo elemento del array de argumentos)
            string command = args[1];

            // Verifica el comando y realiza la acci�n correspondiente
            if (command == "openTextToolApp")
            {
                // Realiza la acci�n deseada, como abrir la aplicaci�n
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
