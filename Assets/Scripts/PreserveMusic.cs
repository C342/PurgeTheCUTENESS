using UnityEngine;
using UnityEngine.SceneManagement;

public class PreserveMusic : MonoBehaviour
{
    GameObject BGM;

    void Awake()
    {
        BGM = GameObject.Find("BGM");

        if (BGM == null)
        {
            BGM = this.gameObject;
            gameObject.name = "BGM";
            DontDestroyOnLoad(BGM);
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