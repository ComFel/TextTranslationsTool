//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#pragma once

#include <string>
#include <TextTranslations.h>
#include <iostream>

namespace textTrad
{
	/// Clase para deserilizar las traducciones con la idea / formato de tabla de CSV
	class CsvDeserialization
	{
	public:

		static bool loadFile(const string& filePath, TextTranslations& translations);
	};
}
