using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testKore : MonoBehaviour
{
    public string eventID;
    private int count = 0;

    private void Start()
    {
        Koreographer.Instance.RegisterForEvents(eventID, ChangeRGB); // зЂВс
    }

    private void ChangeRGB(KoreographyEvent koreographyEvent)
    {
        // todo
        Debug.Log(count++);
    }
}
