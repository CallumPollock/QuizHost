using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClientBehaviour.messageReceived += LoadScene;
    }

    void LoadScene(FixedString128Bytes sceneID)
    {
        SceneManager.LoadScene(sceneID.ToString());
    }

}
