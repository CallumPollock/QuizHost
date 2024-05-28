using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ClientBehaviour.LoadScene += LoadScene;
    }

    void LoadScene(Scenes _scene)
    {
        SceneManager.LoadScene((int)_scene);
    }

}
