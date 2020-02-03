using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartType : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        Destroy(this);
    }

    public string scene;

    
    
}
