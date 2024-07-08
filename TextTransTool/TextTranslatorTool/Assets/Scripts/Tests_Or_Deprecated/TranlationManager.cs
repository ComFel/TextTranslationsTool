using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

/// Clase prueba y testeo de comunicacion y uso en Unity de la DLL
public class TranlationManager : MonoBehaviour
{
    [DllImport("AppBackend")]
    private static extern void testFunction();

    [DllImport("AppBackend")]
    private static extern IntPtr createEditor();

    [DllImport("AppBackend")]
    private static extern void deleteEditor(IntPtr editor);

    [DllImport("AppBackend")]
    private static extern IntPtr createList(string lenguage);

    [DllImport("AppBackend")]
    private static extern void AddList(IntPtr editor, IntPtr list);

    [DllImport("AppBackend")]
    private static extern IntPtr GetListByLenguage(IntPtr editor, string lenguage);

    [DllImport("AppBackend")]
    private static extern IntPtr DuplicateList(IntPtr list, string newLenguage);

    [DllImport("AppBackend")]
    private static extern void AddText(IntPtr list, int id, string content);

    [DllImport("AppBackend")]
    private static extern IntPtr GetTextById(IntPtr list, int id);

    [DllImport("AppBackend")]
    private static extern IntPtr GetContent(IntPtr texto);

    [DllImport("AppBackend")]
    private static extern void SetContent(IntPtr text, string newContent);

    private IntPtr editor;

    void Start()
    {
        editor = createEditor();
        if (editor == IntPtr.Zero)
        {
            Debug.LogError("Failed to create EditorDeTraducciones.");
            return;
        }

        IntPtr listaIngles = createList("Inglés");
        if (listaIngles == IntPtr.Zero)
        {
            Debug.LogError("Failed to create English list.");
            return;
        }

        AddText(listaIngles, 1, "Hello");
        AddText(listaIngles, 2, "Goodbye");

        IntPtr listaEspanol = DuplicateList(listaIngles, "Español");
        if (listaEspanol == IntPtr.Zero)
        {
            Debug.LogError("Failed to replicate Spanish list.");
            return;
        }

        IntPtr texto = GetTextById(listaEspanol, 1);
        if (texto == IntPtr.Zero)
        {
            Debug.LogError("Failed to get text by ID.");
            return;
        }

        IntPtr contenido = GetContent(texto);
        if (contenido == IntPtr.Zero)
        {
            Debug.LogError("Failed to get content.");
            return;
        }

        string contenidoTexto = Marshal.PtrToStringAnsi(contenido);
        Debug.Log($"Texto en español (ID 1): {contenidoTexto}");

        SetContent(texto, "Hola");
        contenido = GetContent(texto);
        contenidoTexto = Marshal.PtrToStringAnsi(contenido);
        Debug.Log($"Texto modificado en español (ID 1): {contenidoTexto}");

        Debug.Log("Spanish list replicated and modified successfully.");

    }

    void OnDestroy()
    {
        if (editor != IntPtr.Zero)
        {
            deleteEditor(editor);
        }
    }
}
