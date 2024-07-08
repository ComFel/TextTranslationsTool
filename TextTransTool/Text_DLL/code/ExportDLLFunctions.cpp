//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#include "ExportDLLFunctions.h"

namespace textTrad
{
	extern "C"
	{
		TextTranslations* createEditor()
		{
			try
			{
				return new TextTranslations();
			}
			catch (...)
			{
				return nullptr;
			}
		}

		void deleteEditor(TextTranslations* editor)
		{
			if (editor)
				delete editor;
		}

		void addText(TextTranslations* editor, const char* textId, const char* lenguageCode, const wchar_t* translation)
		{
			if (editor) 
			{
				wstring _translation = translation;

				editor->AddText(textId, lenguageCode, _translation);
			}
		}
		
		const wchar_t* getTextTranslation(TextTranslations* editor, const char* lenguageCode, const char* textId) 
		{
			if (editor)
			{
				const Text* translation = editor->GetTextTranslation(textId, lenguageCode);

				if (translation) 
				{
					const wstring* textTranslation = translation->GetText(lenguageCode);

					if (textTranslation)
						return textTranslation->c_str();
				}
			}
			return nullptr;
		}

		int GetAllTextTranslations(TextTranslations* editor, TranslationEntry* entries, int maxCount)
		{
			const auto& allTexts = editor->GetAllTexts();
			int count = 0;

			for (const auto& textPair : allTexts)
			{
				const std::string& textId = textPair.first;
				const Text& text = textPair.second;

				for (const auto& langPair : text.GetAllTranslations())
				{
					if (count >= maxCount)
						return count; // Return the number of filled entries

					entries[count].textId = textId.c_str();
					entries[count].languageCode = langPair.first.c_str();
					entries[count].translation = langPair.second.c_str();
					++count;
				}
			}

			return count; // Return the total number of entries
		}

		bool saveTextsToCSV(const char* filePath, TextTranslations* editor, const char** languageCodes, int languageCount)
		{
			string path(filePath);
			vector<string> languages(languageCodes, languageCodes + languageCount);
			return CsvSerialization::saveFile(path, *editor, languages);
		}

		bool loadTextsFromCSV(const char* filePath, TextTranslations* editor)
		{
			string path(filePath);
			return CsvDeserialization::loadFile(path, *editor);
		}
	}
}