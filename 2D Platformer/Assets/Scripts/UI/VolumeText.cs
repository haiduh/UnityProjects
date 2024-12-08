using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; //Sound or Music text
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    private void Update()
    {
        updateVolume();
    }
    private void updateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100; // Retrieve the stored volume and convert to percentage
        txt.text = textIntro + ": " + volumeValue.ToString("0"); // Show the value as an integer
    }
}
