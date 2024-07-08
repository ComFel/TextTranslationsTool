using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// Clase que encapsula las llamadas a la DLL y
/// proporciona una interfaz sencilla para las operaciones de traduccion 
public class TranslationEditor : MonoBehaviour
{
    #region DLL import functions

    [DllImport("AppBackend")]
    private static extern IntPtr createEditor();

    [DllImport("AppBackend")]
    private static extern void deleteEditor(IntPtr editor);

    [DllImport("AppBackend")]
    private static extern void addText(IntPtr editor, string textId, string languageCode, [MarshalAs(UnmanagedType.LPWStr)] string translation);

    [DllImport("AppBackend")]
    private static extern IntPtr getTextTranslation(IntPtr editor, string languageCode, string textId);

    [DllImport("AppBackend")]
    private static extern int GetAllTextTranslations(IntPtr editor, IntPtr entries, int maxCount);

    [DllImport("AppBackend")]
    private static extern bool saveTextsToCSV([MarshalAs(UnmanagedType.LPStr)] string filePath, IntPtr editor, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] languageCodes, int languageCount);

    [DllImport("AppBackend")]
    private static extern bool loadTextsFromCSV([MarshalAs(UnmanagedType.LPStr)] string filePath, IntPtr editor);

    #endregion

    private IntPtr editor;
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct TranslationEntry
    {
        public IntPtr textId;
        public IntPtr languageCode;
        public IntPtr translation;
    }

    public TranslationEditor()
    {
        editor = createEditor();
    }

    public bool IsInitialized => editor != IntPtr.Zero;

    public void Deletion()
    {
        if (editor != IntPtr.Zero)
        {
            deleteEditor(editor);
            editor = IntPtr.Zero;
        }
    }

    public void ClearTranslations()
    {
        translations.Clear();
    }

    /// Funcion encargada de llamar al addtext de la dll y añadir las filas de traducciones de la dll a unity para poder ser usadas
    public void AddText(string textId, string languageCode, string translation)
    {
        if (string.IsNullOrEmpty(textId) || string.IsNullOrEmpty(languageCode) || string.IsNullOrEmpty(translation))
            return;

        addText(editor, textId, languageCode, translation);

        if (!translations.ContainsKey(textId))
        {
            translations[textId] = new Dictionary<string, string>();
        }

        translations[textId][languageCode] = translation;
    }

    /// Funcion de guardado del csv
    public bool SaveToCSV(string filePath, List<string> lenguageCodes)
    {
        if (editor == IntPtr.Zero || lenguageCodes == null || lenguageCodes.Count == 0)
            return false;

        string[] langCodesArray = lenguageCodes.ToArray();
        return saveTextsToCSV(filePath, editor, langCodesArray, langCodesArray.Length);
    }

    /// Funcion de carga del csv
    public bool LoadFromCSV(string filePath)
    {
        bool result = loadTextsFromCSV(filePath, editor);
        if (result)
        {
            translations.Clear();

            var allTranslations = GetAllTranslations();
            foreach (var pair in allTranslations)
            {
                translations[pair.Key] = pair.Value;
            }
        }
        else
        {
            Debug.LogWarning("Failed to load translations from CSV using DLL.");
        }
        return result;
    }

    /// Funcion que se encarga de iterar sobre todas las traducciones, para primero obtenerlas todas y lo mas
    /// principal, el separarlas por sus categorias correspondientes (id, traduccion 1, traduccion 2)(el struct de la dll y aqui)
    public Dictionary<string, Dictionary<string, string>> GetAllTranslations()
    {
        const int maxItems = 1000; // Numero "magico"
        TranslationEntry[] entries = new TranslationEntry[maxItems];
        GCHandle handle = GCHandle.Alloc(entries, GCHandleType.Pinned);
        IntPtr ptr = handle.AddrOfPinnedObject();

        int translationCount = GetAllTextTranslations(editor, ptr, maxItems);

        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

        // Separacion de la linea en sus respectivos contenedores para luego ser mostrado y guardado
        for (int i = 0; i < translationCount; i++)
        {
            string textId = Marshal.PtrToStringAnsi(entries[i].textId);
            string languageCode = Marshal.PtrToStringAnsi(entries[i].languageCode);
            string translation = Marshal.PtrToStringUni(entries[i].translation);

            if (!result.ContainsKey(textId))
            {
                result[textId] = new Dictionary<string, string>();
            }

            result[textId][languageCode] = translation;
        }

        handle.Free();
        return result;
    }

    /// Funcion para obtener una lista de los idiomas que se usan en el csv
    public List<string> GetAllLanguageCodes()
    {
        HashSet<string> languageCodes = new HashSet<string>();

        foreach (var langs in translations.Values)
        {
            foreach (var lang in langs.Keys)
            {
                languageCodes.Add(lang);
            }
        }

        return new List<string>(languageCodes);
    }
}
