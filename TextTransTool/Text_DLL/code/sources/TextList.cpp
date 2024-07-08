//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.5


#include <TextList.h>

namespace textTrad
{
	TextList::TextList(const string& lenguage) : lenguage(lenguage) {}

	void TextList::AddText(const Text& text) { texts.push_back(text); }

	Text* TextList::GetTextByID(int id)
	{
		for (auto& text : texts)
		{
			if (text.GetId() == id)  return &text;
		}

		return nullptr;
	}

	const string& TextList::GetLenguage() const { return lenguage; }

	vector<Text> TextList::GetTexts() const { return texts; }

	TextList TextList::duplicateTexts(const string& newLenguage) 
	{
		TextList newTexts(newLenguage);

		for (const auto& text : texts)
		{
			newTexts.AddText(text);
		}
		return newTexts;
	}
}
