# basic_ollama_chat

To get started,
* Install Ollama `winget install Ollama`
* Get a new model for Ollama, like `ollama pull llama3.2` or `ollama pull deepseek-r1`
* (note, at this point you can test it by running `ollama run llama3.2` or with deepseek)
* Update the code to use the model you chose:
  ```csharp
   ollama.SelectedModel = "deepseek-r1";
  ```
