//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#pragma once

#include <Text.h>
#include <string>
#include <map>


namespace textTrad
{
	/// Struct para manejar la carga del csv y ordenar la informacion en su categoria correspondiente,
	/// asi mismo, para poder se manejado correctamente en Unity (C#)
	struct TranslationEntry
	{
		const char*	   textId;
		const char*	   languageCode;
		const wchar_t* translation;
	};

	/// Gestiona multiples Textos, identificados un ID unico
	class TextTranslations
	{
	private:

		map < string, Text > textsTranslations;

		string activeLenguageCode;

	public:

		void AddText(const string& textID, const string& lenguageCode, const wstring& translation);

		const Text* GetTextTranslation(const string& textID, const string& lenguageCode) const;

		const map <string, Text>& GetAllTexts() const;
	};
}
