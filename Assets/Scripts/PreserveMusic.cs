using UnityEngine;
using UnityEngine.SceneManagement;

public class PreserveMusic : MonoBehaviour
{
    void Awake()
    {
        musicPlayer = GameObject.Find("BGM");

        if (musicPlayer == null)
        {
            musicPlayer = this.gameObject;
            musicPlayer.name = "BGM";
            DontDestroyOnLoad(musicPlayer);
        }
        else
        {
            if (this.gameObject.name != "BGM")
            {
                Destroy(this.gameObject);
            }
        }

    }
}