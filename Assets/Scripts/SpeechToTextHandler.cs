using UnityEngine;
using OpenAI;
using System;
using TMPro;
using UnityEngine.UI;
using OpenAI.Audio;
using Utilities.Audio;
using Utilities.Encoding.Wav;

public class SpeechToTextHandler : MonoBehaviour
{
    [SerializeField] string apiKey;
    [SerializeField] Button recordButton;
    [SerializeField] TMP_Text buttonText;

    OpenAIClient api;

    private void Start()
    {
        api = new OpenAIClient(apiKey);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRecordAudio();
        }
    }

    public void ToggleRecordAudio()
    {
        if (RecordingManager.IsRecording)
        {
            RecordingManager.EndRecording();
            buttonText.text = "Record";
        }
        else
        {
            buttonText.text = "Recording";
            RecordingManager.StartRecording<WavEncoder>(callback: ProcessRecording);
        }
    }

    private async void ProcessRecording(Tuple<string, AudioClip> recording)
    {
        var (path, clip) = recording;
        recordButton.interactable = false;

        try
        {
            var request = new AudioTranscriptionRequest(clip, temperature: 0.1f, language: "en");
            var userInput = await api.AudioEndpoint.CreateTranscriptionTextAsync(request, destroyCancellationToken);

            GetComponent<ClientUser>().AskQuestion(userInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            recordButton.interactable = true;
        }
        finally
        {
            recordButton.interactable = true;
        }
    }

}
