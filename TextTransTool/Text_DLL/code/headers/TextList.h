//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.5

#pragma once

#include <Text.h>
#include <vector>
#include <string>


namespace textTrad
{
	class TextList
	{

	private:

		string		 lenguage;
		vector<Text> texts;

	public:

		TextList(const string& lenguage);

		void AddText(const Text& text);
		
		Text*		  GetTextByID(int id);
		const string& GetLenguage() const;
		vector<Text>  GetTexts() const;

		TextList duplicateTexts(const string& newLenguage);

	};
}

