using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Clase encargada de gestionar los elementos de la UI y 
/// la comunicacion con el TranslationEditor(DLL)
public class UIManager : MonoBehaviour
{    
    [SerializeField] private Button buttonAdd;          // Añadir fila
    [SerializeField] private Button buttonSave;         // Guardar csv
    [SerializeField] private Button buttonLoad;         // Carga csv
    [SerializeField] private Button buttonAddLenguage;  // Añadir idioma (usando ISO 639)
    [SerializeField] private TMP_InputField inputLenguage; // Input del code de idioma relacionado al boton

    [SerializeField] private TMP_Text statusText;        // Salida tipo consola con logs

    [SerializeField] private RectTransform tableContent; // Contenedor filas tabla
    [SerializeField] private GameObject rowPrefabGO;     // Prefab fila

    [SerializeField] private Button buttonExit;          // Salir App

    private TranslationEditor translationEditor;

    private List<string> languageCodes;                  // Lista de codigos de idioma

    private void Start()
    {
        translationEditor = gameObject.AddComponent<TranslationEditor>(); // new TranslationEditor();

        if (!translationEditor.IsInitialized)
        {
            Debug.LogWarning("Failed to initialize TranslationEditor.");
            return;
        }

        languageCodes = translationEditor.GetAllLanguageCodes();

        // Listeners de las funciones
        buttonAddLenguage.onClick.AddListener(AddLenguageCode);
        buttonAdd.onClick.AddListener(AddNewRow);
        buttonSave.onClick.AddListener(SaveToCSV);
        buttonLoad.onClick.AddListener(LoadFromCSV);
        buttonExit.onClick.AddListener(ExitApp);
    }

    private void OnDestroy()
    {
        if (translationEditor != null)
            translationEditor.Deletion();
    }

    // Salir de la aplicacion 
    private void ExitApp()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /// Funcion para añadir idiomas (con iso 639)
    private void AddLenguageCode()
    {
        string code = inputLenguage.text.Trim();

        if (!string.IsNullOrEmpty(code) && !languageCodes.Contains(code))
        {
            languageCodes.Add(code);
            inputLenguage.text = "";
            statusText.text = "Language code added.";
        }
        else
        {
            statusText.text = "Invalid or duplicate language code.";
        }
    }

    /// Funcion para crear una fila con datos vacios
    private void AddNewRow()
    {
        List<string> emptyTranslations = new List<string>(new string[languageCodes.Count]);
        AddNewRow("", emptyTranslations);
    }

    /// Funcion para crear una fila con datos especificos
    private void AddNewRow(string textId, List<string> translations)
    {
        GameObject newRow = Instantiate(rowPrefabGO, tableContent.transform);

        // Asignar el ID de texto a la primera celda
        TMP_InputField textIdField = newRow.transform.GetChild(0).GetComponent<TMP_InputField>();
        textIdField.text = textId;
        textIdField.onEndEdit.AddListener((newValue) => OnTextIdEdit(newRow, newValue));

        // Asignar las traducciones a las siguientes celdas
        for (int i = 0; i < translationEditor.GetAllTranslations().Count; i++)
        {
            TMP_InputField inputField = newRow.transform.GetChild(i + 1).GetComponent<TMP_InputField>();
            inputField.text = i < translations.Count ? translations[i] : "";
            int languageIndex = i; // Ajustar el índice para que coincida con las celdas de traducción
            inputField.onEndEdit.AddListener((newValue) => OnCellEdit(inputField.gameObject, languageIndex, newValue));            
        }
    }

    /// Funcion para modificar el valor de un celda 
    private void OnTextIdEdit(GameObject row, string textId)
    {
        var inputFields = row.GetComponentsInChildren<TMP_InputField>();

        if (inputFields.Length > 0)
        {
            var oldTextId = inputFields[0].text;
            if (!string.IsNullOrEmpty(oldTextId) && oldTextId != textId)
            {
                // Actualizar todas las celdas correspondientes al nuevo textId
                for (int i = 1; i < inputFields.Length; i++)
                {
                    string newValue = inputFields[i].text;
                    OnCellEdit(row, i - 1, newValue);
                }
            }
        }
    }

    /// Funcion para actualizar el valor de una celda
    private void OnCellEdit(GameObject row, int languageIndex, string newValue)
    {
        // Ajustar languageIndex para tener en cuenta que la primera celda es el ID
        int adjustedLanguageIndex = languageIndex - 1;

        if (adjustedLanguageIndex < 0 || adjustedLanguageIndex >= languageCodes.Count)
        {
            Debug.LogError("Índice de idioma fuera de rango.");
            return;
        }

        // Obtener el ID de texto de la primera celda del row correspondiente
        string textId = row.transform.parent.GetChild(0).GetComponent<TMP_InputField>().text;

        // Obtener el código de idioma correspondiente al índice ajustado
        string languageCode = languageCodes[adjustedLanguageIndex];

        // Actualizar la traducción en el editor de traducción
        translationEditor.AddText(textId, languageCode, newValue);
    }

    /// Funcion que para avisar a la dll para guardar
    private void SaveToCSV()
    {
        // Recopilar datos de la tabla antes de guardar
        CollectTableData();

        foreach (Transform rowTransform in tableContent)
        {
            var inputFields = rowTransform.GetComponentsInChildren<TMP_InputField>();
            if (inputFields.Length > 0)
            {
                string textId = inputFields[0].text;
                for (int i = 1; i < inputFields.Length; i++)
                {
                    string translation = inputFields[i].text;
                    string languageCode = languageCodes[i - 1];
                    translationEditor.AddText(textId, languageCode, translation);
                }
            }
        }

        string filePath = Application.dataPath + "/Translations.csv";
        bool success = translationEditor.SaveToCSV(filePath, languageCodes);

        if (success)
        {
            statusText.text = "Translations saved to CSV.";
        }
        else
        {
            statusText.text = "Failed to save translations.";
        }
    }

    // Actualizar tabla creada por primera vez antes de ser creado el archivo CSV
    private void CollectTableData()
    {
        // Recopilar datos de la tabla y actualizar TranslationEditor
        translationEditor.ClearTranslations();

        foreach (Transform rowTransform in tableContent)
        {
            GameObject row = rowTransform.gameObject;
            TMP_InputField textIdField = row.transform.GetChild(0).GetComponent<TMP_InputField>();
            string textId = textIdField.text;

            for (int i = 0; i < languageCodes.Count; i++)
            {
                TMP_InputField translationField = row.transform.GetChild(i + 1).GetComponent<TMP_InputField>();
                string translation = translationField.text;
                string languageCode = languageCodes[i];

                translationEditor.AddText(textId, languageCode, translation);
            }
        }
    }

    /// Funcion que para avisar a la dll para cargar
    private void LoadFromCSV()
    {        
        string filePath = Application.dataPath + "/Translations.csv";
        bool success = translationEditor.LoadFromCSV(filePath);

        //string fileName = "Translations";
        //bool success2 = translationEditor.LoadFromResources(fileName);

        if (success)
        {
            statusText.text = "Translations loaded from CSV.";

            // Obtener los lenguajes del archivo
            languageCodes = translationEditor.GetAllLanguageCodes();            

            UpdateTable();
        }
        else
        {
            statusText.text = "Failed to load translations.";
        }
    }

    /// Funcion de actualizacion de la tabla que se muestra en pantalla, con la que interactua el usuario 
    private void UpdateTable()
    {
        // Limpiar filas existentes
        foreach (Transform child in tableContent)
        {
            Destroy(child.gameObject);
        }

        var allTexts = translationEditor.GetAllTranslations();

        foreach (var textEntry in allTexts)
        {
            string textId = textEntry.Key;
            Dictionary<string, string> translations = textEntry.Value;

            // Crear una lista ordenada de traducciones según los códigos de idioma disponibles
            List<string> translationValues = new List<string>();

            // Recorremos los codigos de idiomas para buscar si por su id tiene traduccion
            foreach (var languageCode in languageCodes)
            {
                if (translations.ContainsKey(languageCode))
                {
                    translationValues.Add(translations[languageCode]);
                }
                else
                {
                    translationValues.Add(""); // Si no hay traducción para el idioma, añadir cadena vacía
                }
            }

            // Crear una nueva fila y asignar el ID y las traducciones
            AddNewRow(textId, translationValues);            
        }
    }
}