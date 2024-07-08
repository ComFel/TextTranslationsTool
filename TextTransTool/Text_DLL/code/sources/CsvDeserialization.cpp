//
// Felipe Vallejo Molina
// felipevm07@gmail.com
// 2024.6

#include <CsvDeserialization.h>
#include <fstream>
#include <sstream>
#include <vector>

namespace textTrad
{
    bool CsvDeserialization::loadFile(const string& filePath, TextTranslations& translations) 
    {
        ifstream file(filePath);
        if (!file.is_open()) 
        {
            cout << "Failed to open file: " << filePath << endl;
            return false;
        }

        string line;
        vector< string> languageCodes;

        // Leer la cabecera
        if (getline(file, line)) 
        {
            istringstream headerLine(line);
            string header;
            getline(headerLine, header, ','); // Omitir "id"

            while ( getline(headerLine, header, ',')) 
            {
                languageCodes.push_back(header);
                cout << "Header: " << header << endl; // Depuración
            }
        }

        // Leer cada línea (ID del texto y traducciones)
        while (getline(file, line)) 
        {
             istringstream line_stream(line);
             string text_id;

             getline(line_stream, text_id, ',');

             cout << "Text ID: " << text_id << endl; // Depuración

            for (size_t i = 0; i < languageCodes.size(); ++i) 
            {
                string translation;

                if (getline(line_stream, translation, ',')) 
                {
                    wstring w_translation(translation.begin(), translation.end());
                    translations.AddText(text_id, languageCodes[i], w_translation);
                    cout << "Translation for " << languageCodes[i] << ": " << translation << endl; // Depuracións
                }
            }
        }

        file.close();
        return true;
    }
}