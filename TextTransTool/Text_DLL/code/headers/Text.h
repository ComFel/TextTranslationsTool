//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#pragma once

#include <string>
#include <map>

using namespace std;

namespace textTrad 
{
	/// Representacion y gestion de un texto traducido en un lenguage definido (por ISO - 639)
	class Text
	{
	private:

		map < string, wstring > translationsMap;
		
		// Mutable para permitir modificacion en funciones const
		mutable string	 lastLenguageCode;
		mutable wstring* lastTranslation = nullptr;
		

	public:
		
		void AddTranslation(const string& languageCode, const wstring& translation);

		const wstring* GetText(const string& lenguageCode) const;

		const map<string, wstring>& GetAllTranslations() const;
	};
}

