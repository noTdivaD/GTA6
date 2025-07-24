using UnityEngine;

public class KarenSFXPlay : MonoBehaviour
{
  
    public string karenName = "MyNameKaren"; // Name of the character, not used in this script but can be useful for identification
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (karenName != null && karenName != "")
        {
            AudioClip karenClip = AudioManager.Instance.GetClipByName(karenName, AudioManager.Instance.characterSfxClips);
            if ( karenClip!= null)
            {
                AudioManager.Instance.PlayCharacterSFX(karenClip);
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
