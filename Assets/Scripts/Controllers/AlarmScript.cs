using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour
{
    [SerializeField] private AudioSource PA;
    [SerializeField] private AudioSource AB;

    [SerializeField] private Animator myAnimator;

    public bool isPlaying;

    private void Start()
    {
        myAnimator = transform.GetComponent<Animator>();
        isPlaying = false;
    }

    public void StartAlarm()
    {
        isPlaying = true;
        PA.Play();
        AB.Play();

        myAnimator.SetBool("alarmActive", true);
    }

}
