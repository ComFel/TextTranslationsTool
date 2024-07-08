//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#pragma once

#include <string>
#include <TextTranslations.h>
#include <vector>

namespace textTrad
{
	/// Clase para serilizar las traducciones con la idea / formato de tabla de CSV
	class CsvSerialization 
	{
	public:

		static bool saveFile(const string& filePath, TextTranslations& translations, const vector<string>& languageCodes);

	};
}
