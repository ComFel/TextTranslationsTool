//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#pragma once

#include <TextTranslations.h>
#include <iostream>
#include <CsvSerialization.h>
#include <CsvDeserialization.h>
#include <vector>

#ifdef DLL_EXPORT
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

namespace textTrad
{
	extern "C"
	{
		/// Crear una nueva instancia de TextTranslations 
		DLL_API TextTranslations* createEditor          ();
		
		/// Destruir una instancia de TextTranslations
		DLL_API void              deleteEditor	        (TextTranslations* editor);
		
		/// Añadir una traducción a un texto específico identificado por textId
		DLL_API void              addText				(TextTranslations* editor, const char* textId,const char* lenguageCode, const wchar_t* translation);
		
		/// Obtener la traducción de un texto específico en un idioma específico
		DLL_API const wchar_t*	  getTextTranslation	(TextTranslations* editor, const char* lenguageCode, const char* textId);
		
		DLL_API int				  GetAllTextTranslations(TextTranslations* editor, TranslationEntry* entries, int maxCount);

		/// Guardar archivo de traducciones como CSV
		DLL_API bool			  saveTextsToCSV		(const char* filePath, TextTranslations* editor, const char** languageCodes, int languageCount);

		/// Cargar archivo de traducciones tipo CSV
		DLL_API bool			  loadTextsFromCSV		(const char* filePath, TextTranslations* editor);
	}
}