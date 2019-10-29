using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public static BGMusic music;

    void Awake()
    {
        JustMonika();
    }

    private void JustMonika()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (music != this) Destroy(gameObject);
    }
}
