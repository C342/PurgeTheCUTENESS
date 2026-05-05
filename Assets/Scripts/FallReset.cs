using UnityEngine;
using UnityEngine.SceneManagement;

public class FallReset : MonoBehaviour
{
    public float fallLimitY = -10f;
    public float resetDelay = 0.5f;

    private bool isResetting = false;

    void Update()
    {
        if (isResetting) return;

        if (transform.position.y < fallLimitY)
        {
            StartCoroutine(RestartLevel());
        }
    }

    System.Collections.IEnumerator RestartLevel()
    {
        isResetting = true;

        yield return new WaitForSeconds(resetDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}