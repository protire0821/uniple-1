using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //点击鼠标右键切换场景
        if (Input.GetKeyDown(KeyCode.Return))
        {
          //  Application.LoadLevel("SampleScene");
            //SceneManager.LoadScene("SampleScene");
        }
    }
}
