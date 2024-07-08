//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#include <CsvSerialization.h>
#include <fstream>
#include <set>
#include <codecvt>

namespace textTrad
{
	bool CsvSerialization::saveFile(const string& filePath, TextTranslations& translations, const vector<string>& languageCodes)
	{
		ofstream file(filePath);
		if (!file.is_open())
			return false;

        // Escribir la cabecera (ID, lenguageCode1, lenguageCode2, ...)
        file << "id";

        for (const auto& lang_code : languageCodes)
        {
            file << "," << lang_code;
        }
        file << "\n";

        // Escribir cada texto con sus traducciones
        for (const auto& pair : translations.GetAllTexts())
        {
            const auto& textId = pair.first;
            const auto& text = pair.second;

            file << textId;

            for (const auto& lang_code : languageCodes)
            {
                const auto* translation = text.GetText(lang_code);

                file << ",";

                if (translation)
                {
                    wstring_convert<codecvt_utf8<wchar_t>> converter;
                    string convertedTranslation(converter.to_bytes(*translation));
                    file << convertedTranslation;
                }
            }
            file << "\n";
        }

        file.close();
        return true;
	}
}