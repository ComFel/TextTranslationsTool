//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#include <Text.h>

namespace textTrad
{
	void Text::AddTranslation(const string& lenguageCode, const wstring& translation)
	{
		translationsMap[lenguageCode] = translation;
	}

	/// Busqueda del texto, esta implementado con una forma simple de caché para ahorrar tiempo
	const wstring* Text::GetText(const string& lenguageCode) const
	{
        // Check cache
        if (lastLenguageCode == lenguageCode)
        {
            return lastTranslation;
        }
        // Update cache
        else
        {
            lastLenguageCode = lenguageCode;

            auto item = translationsMap.find(lenguageCode);

            if (item != translationsMap.end())
            {
                // casteo por compatibilidad
                return const_cast<wstring*>(& item->second);
            }
            else 
            {
                return lastTranslation = nullptr;
            }
        }
	}

    const map<string, wstring>& Text::GetAllTranslations() const
    {
        return translationsMap;
    }
}