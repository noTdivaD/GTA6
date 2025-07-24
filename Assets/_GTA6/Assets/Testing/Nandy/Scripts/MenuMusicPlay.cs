using UnityEngine;

public class MenuMusicPlay : MonoBehaviour
{
    public string musicClipName = "MenuMainTheme"; // Name of the music sound effect
    public string karenName = "MyNameKaren"; // Name of the character, not used in this script but can be useful for identification
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(musicClipName != null && musicClipName != "")
        {
            AudioClip musicClip = AudioManager.Instance.GetClipByName(musicClipName, AudioManager.Instance.menuMusicClips);
            if (musicClip != null)
            {
                AudioManager.Instance.PlayMenuMusic(musicClip);
            }
         
            
        }
        //if (karenName != null && karenName != "")
        //{
        //    AudioClip karenClip = AudioManager.Instance.GetClipByName(musicClipName, AudioManager.Instance.characterSfxClips);
        //    if ( karenClip!= null)
        //    {
        //        AudioManager.Instance.PlayCharacterSFX(karenClip);
        //    }


        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
