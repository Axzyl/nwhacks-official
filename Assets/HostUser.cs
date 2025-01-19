using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostUser : MonoBehaviour
{
    private ApiHandler apiHandler;
    public LLMCharacter character;

    async void Start()
    {
        apiHandler = gameObject.GetComponent<ApiHandler>();

        // Example texts
        List<string> exampleTexts = new List<string>
        {
            "Quantum computing is a type of computation that takes advantage of quantum mechanics.",
            "A quantum bit (or qubit) is the fundamental unit of quantum information.",
            "Superposition and entanglement are two essential features of quantum computing."
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
    }

}
