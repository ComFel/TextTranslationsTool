//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6


#include <TextTranslations.h>

namespace textTrad
{
	void TextTranslations::AddText(const string& textID, const string& lenguageCode, const wstring& translation)
	{
		textsTranslations[textID].AddTranslation(lenguageCode, translation);
	}


	const Text* TextTranslations::GetTextTranslation(const string& textID, const string& lenguageCode) const
	{
		auto item = textsTranslations.find(textID);

		if (item != textsTranslations.end()) 
		{
			return &item->second;
		}
		else
			return nullptr;
	}

	const map <string, Text>& TextTranslations::GetAllTexts() const
	{
		return textsTranslations;
	}
}