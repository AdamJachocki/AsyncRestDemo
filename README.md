Ten projekt jest przykładowym kodem dla artykułu opisującego asynchroniczne REST API: https://masterbranch.pl/asynchroniczne-rest-api-jak-i-po-co/

# Azure
Pełne wykorzystanie kodu wymaga subskrypcji Azure'owej. W katalogu __deployment__ znajdują się pliki BICEP, tworzące wymagane usługi na Azure.

# Opcje
Musisz wypełnić swoje opcje odpowiednimi wartościami. Głównie chodzi o connection stringi do usług na Azure.

# Projekty
Główny kod serwera to projekt AsyncServer. Natomiast kod wykorzystuje jeszcze funkcje azurowe - projekt __AzureFunctions__

# UWAGA!
Funkcję azurową musisz opublikować na własnej subskrypcji Azure lub korzystać z lokalnego symulatora.
