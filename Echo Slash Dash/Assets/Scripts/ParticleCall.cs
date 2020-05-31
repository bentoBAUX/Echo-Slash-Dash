using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCall : MonoBehaviour
{
    public GameObject SwordTrail;
    public GameObject SwordSpark;
    void Start()
    {
        SwordTrail.SetActive(false);

    }
    void SwordTrailEnable()
    {
        SwordTrail.SetActive(true);
    }

    void SwordTrailDisable()
    {
        SwordTrail.SetActive(false);
    }

}
