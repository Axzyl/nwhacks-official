using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HostUser : MonoBehaviour
{
    private ApiHandler apiHandler;
    public LLMCharacter character;

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text logText;

    async void Start()
    {
        
    }

    public async void UploadData()
    {
        apiHandler = gameObject.GetComponent<ApiHandler>();

        // Example texts
        List<string> exampleTexts = new List<string>
        {
            inputField.text
        };

        // Example embeddings
        List<List<float>> exampleEmbeddings = new List<List<float>>();
        for (int i = 0; i < exampleTexts.Count; i++)
        {
            // Generate a random embedding of size 384
            List<float> embedding = await character.Embeddings(exampleTexts[i]);
            exampleEmbeddings.Add(embedding);
        }

        // Upload data to the API
        StartCoroutine(UploadDataToApi(exampleTexts, exampleEmbeddings));
    }

    private IEnumerator UploadDataToApi(List<string> texts, List<List<float>> embeddings)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            string text = texts[i];
            List<float> embedding = embeddings[i];

            // Call the API upload function
            yield return apiHandler.UploadData(text, embedding);
        }

        logText.text = "Database Successfully Updated";

        yield return new WaitForSeconds(2);

        logText.text = "";
    }

}
