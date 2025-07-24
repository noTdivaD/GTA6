using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    public string musicClipName = "MainTheme"; // Name of the music sound effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(musicClipName != null && musicClipName != "")
        {
            AudioClip musicClip = AudioManager.Instance.GetClipByName(musicClipName, AudioManager.Instance.musicClips);
            if (musicClip != null)
            {
                AudioManager.Instance.PlaySFX(musicClip);
            }
         
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
