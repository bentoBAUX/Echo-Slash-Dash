using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastWave : MonoBehaviour
{
    public GameObject Wave;
    public Transform Blade;
    public Transform BladeROT;
    public Transform BladePOS;
    public float CastForce;
    
    
    public Camera cam;
    Animator animator;
    void GetRot()
    {
        animator = GetComponent<Animator>();

        BladeROT.rotation = Blade.rotation;
    }
    void CastWaves()
    {
        Rigidbody WaveInstance;
        WaveInstance = Instantiate(Wave.GetComponent<Rigidbody>(), BladePOS.position, BladeROT.rotation) as Rigidbody;
        WaveInstance.GetComponent<Rigidbody>().isKinematic = false;
        WaveInstance.transform.parent = null;
        WaveInstance.GetComponent<Rigidbody>().AddForce(cam.transform.forward * CastForce, ForceMode.Impulse);
        Destroy(WaveInstance.gameObject, 2);
        animator.SetTrigger("Attack Out");
    }

    
}
