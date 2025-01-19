using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHandler : MonoBehaviour
{
    public string UploadEndpoint = "http://127.0.0.1:5000/upload";
    public string RetrieveEndpoint = "http://127.0.0.1:5000/retrieve";

    [System.Serializable]
    public class ApiPayload
    {
        public string text;
        public List<float> embedding;

        public ApiPayload(string text, List<float> embedding)
        {
            this.text = text;
            this.embedding = embedding;
        }
    }

    [System.Serializable]
    public class ApiRecieve
    {
        public List<float> query_embedding;

        public ApiRecieve(List<float> query_embedding)
        {
            this.query_embedding = query_embedding;
        }
    }

    // Upload data to the API
    public IEnumerator UploadData(string text, List<float> embedding)
    {
        // Create an instance of ApiPayload
        var payload = new ApiPayload(text, embedding);

        // Serialize to JSON
        string jsonPayload = JsonUtility.ToJson(payload);

        // Make the POST request
        using (UnityWebRequest request = new UnityWebRequest(UploadEndpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // Handle response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data uploaded successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to upload data: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class RetrieveResponse
    {
        public List<string> retrieved; // Retrieved texts
    }

    // Retrieve data from the API
    public IEnumerator RetrieveData(string query, List<float> queryEmbedding)
    {
        // Create an instance of ApiPayload
        var payload = new ApiRecieve(queryEmbedding);

        // Serialize to JSON
        string jsonPayload = JsonUtility.ToJson(payload);

        // Make the POST request
        using (UnityWebRequest request = new UnityWebRequest(RetrieveEndpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // Handle response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data retrieved successfully: " + request.downloadHandler.text);
                
                // Parse the response JSON
                var response = JsonConvert.DeserializeObject<RetrieveResponse>(request.downloadHandler.text);

                // Generate LLM input
                string context = string.Join("\n", response.retrieved);
                string llmInput = $"Context: {context}\n\nQuery: {query}\nAnswer:";
                Debug.Log($"LLM Input: {llmInput}");

                GetComponent<ClientUser>().SendLLM(llmInput);
            }
            else
            {
                Debug.LogError("Failed to retrieve data: " + request.error);
            }
        }
    }
}
