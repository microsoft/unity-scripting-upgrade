using Newtonsoft.Json;
#if UNITY_WEBGL
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Collections;
#else
using System.Net.Http;
using System.Threading.Tasks;
#endif
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

/// <summary>
/// This demonstrates new features available with Unity's .NET 4.x equivalent scripting upgrade
/// including: string interpolation (ln 70), dynamic binding (ln 144), (ln 138) async / await, 
/// and use of the .NET packages Json.net and HttpClient.
/// Note that to use HttpClient you must either use the .NET Standard 2.0 profile in your Player Settings 
/// or include an assembly reference in a mcs.rsp file in your root Assets directory.
/// </summary>
public class Demo : MonoBehaviour
{
	#region Unity editor serialized fields
	[SerializeField]
	private Text outputText, errorMessageText;

	[SerializeField]
	private InputField input;

	[SerializeField]
	private GameObject errorMessagePanel;

	[SerializeField]
	private Button submitButton;

	[SerializeField]
	private AudioSource applause, ambience;

	[SerializeField]
	private ParticleSystem confetti;
	#endregion
	const string questionMarks = "???";
	#if UNITY_WEBGL
#else
	HttpClient client;
#endif

	private void Awake()
	{
		HideErrorMessage();
		ResetForm();
#if UNITY_WEBGL
#else
		client = new HttpClient();
#endif
	}

	/// <summary>
	/// Called from a Unity button event hooked up in the editor.
	/// WebGL does not support threads, so we have to use UnityWebRequest there 
	/// instead of HttpClient.
	/// </summary>
	public void SubmitButtonPressed()
	{
		HideErrorMessage();
		EnableInput(false);
		if (ValidateEntryText(input.text))
		{
			outputText.text = $"Number {input.text}...";
#if UNITY_WEBGL
			StartCoroutine(SendRequestWithUnityWebRequest());
#else
			SendRequestWithHttpClient();
#endif
		}
		else
		{
			ShowErrorMessage("Invalid entry!");
			EnableInput(true);
		}
	}

	/// <summary>
	/// The API only supports entries within this range.
	/// </summary>
	/// <param name="entryText">Text from user input.</param>
	/// <returns>True if entry text is valid. Otherwise false.</returns>
	private bool ValidateEntryText(string entryText)
	{
		const int minValidEntry = 1;
		const int maxValidEntry = 802;
		int entryNumber;
		bool isValidEntry = false;

		if (int.TryParse(entryText, out entryNumber))
		{
			isValidEntry = entryNumber >= minValidEntry && entryNumber <= maxValidEntry;
		}

		return isValidEntry;
	}

#if UNITY_WEBGL
	/// <summary>
	/// Unity coroutine for sending a request to the API with UnityWebRequest.
	/// JObject.Parse is used here because JsonConvert.DeserializeObject crashes on WebGL.
	/// Note that using Unity's JsonUtility does not support arrays directly,
	/// so deserializing without Json.net would be much more of a pain 
	/// (https://answers.unity.com/questions/1123326/jsonutility-array-not-supported.html).
	/// </summary>
	private IEnumerator SendRequestWithUnityWebRequest()
	{
		string url = $"https://www.pokeapi.co/api/v2/pokemon/{input.text}/";
		var request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
			ShowErrorMessage("Something went wrong, please try again later!\n" +
				$"Error: {request.error}");
		else
		{
			Debug.Log($"JSON: {request.downloadHandler.text}");
			var name = JObject.Parse(request.downloadHandler.text)
				.GetValue("name").ToString();
			name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
			outputText.text += $"\nYou got {name}!";
			PlayCelebration();
			yield return new WaitForSeconds(applause.clip.length);
			ResetForm();
		}
	}
#else
	/// <summary>
	/// Sends a request to the API with HttpClient, using async / await syntax.
	/// Note that you can use Try / Catch here, but not in a coroutine.
	/// </summary>
	private async void SendRequestWithHttpClient()
	{
		try
		{
			var json = await client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{input.text}/");
			Debug.Log($"JSON: {json}");
			var obj = JObject.Parse(json);
			var capitalizedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((string)obj["name"]);
			outputText.text += $"\nYou got {capitalizedName}!";
			PlayCelebration();
			await Task.Delay(TimeSpan.FromSeconds(applause.clip.length));
			ResetForm();
		}
		catch (Exception e)
		{
			ShowErrorMessage("Something went wrong, please try again later!\n" +
				$"Error: {e.Message}");
		}
	}
#endif

	private void EnableInput(bool value)
	{
		submitButton.interactable = value;
		input.interactable = value;
	}

	private void PlayCelebration()
	{
		ambience.Stop();
		confetti.Play();
		applause.Play();
	}

	private void ResetForm()
	{
		outputText.text = questionMarks;
		input.text = string.Empty;
		ambience.Play();
		EnableInput(true);
	}

	private void ShowErrorMessage(string message)
	{
		errorMessageText.text = message;
		errorMessagePanel.SetActive(true);
		ResetForm();
	}

	private void HideErrorMessage()
	{
		errorMessagePanel.SetActive(false);
	}
}
