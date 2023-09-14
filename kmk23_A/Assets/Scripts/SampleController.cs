using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.InscreaseScore(10);
        GameManager.Instance.InscreaseScore(15);
    }
    
}
