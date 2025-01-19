using System.Collections.Generic;
using UnityEngine;
using LLMUnity;
using UnityEngine.TextCore.Text;
using System;
using TMPro;
using Unity.Services.Relay;

public class ClientUser : MonoBehaviour
{
    private ApiHandler apiHandler;
    public LLMCharacter character;

    [SerializeField] TMP_Text inputText;

    async void Start()
    {
        AskQuestion("What are essential features of quantum computing?");
    }

    public async void AskQuestion(string query)
    {
        apiHandler = gameObject.GetComponent<ApiHandler>();

        //string query = "What are essential features of quantum computing?";

        List<float> queryEmbedding = await character.Embeddings(query);

        // Retrieve data
        StartCoroutine(apiHandler.RetrieveData(query, queryEmbedding));
    }

    public async void SendLLM(string input)
    {
        await character.Chat(input, HandleReply, ReplyCompleted);
    }

    void HandleReply(string reply)
    {
        inputText.text = reply;
    }

    void ReplyCompleted()
    {
        // do something when the reply from the model is complete
        Debug.Log("The AI replied");
    }


}
