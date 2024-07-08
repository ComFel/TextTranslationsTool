using UnityEngine;
using System;

/// Clase encargada de inicializar TranslatorEditor(DLL) y 
/// gestionar la logica principal de la aplicacion 
/// Deprecated Class, movida logica a UIManager
public class TextTranslationManager : MonoBehaviour
{
    private TranslationEditor translationEditor;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        translationEditor = gameObject.AddComponent<TranslationEditor>(); // new TranslationEditor();

        if (!translationEditor.IsInitialized)
        {
            Debug.LogError("Failed to initialize TranslationEditor.");
            return;
        }

        //uiManager = GetComponent<UIManager>();

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        //uiManager.Initialize(translationEditor);
    }

    private void OnDestroy()
    {
        translationEditor.Deletion();
    }

}
