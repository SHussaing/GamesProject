using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public PlayerMovement pm;
    public PlayerDash pd;
    public AudioSource soundEffectSource;
    public AudioClip Dash;
    public AudioClip Sprint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
/*        if(pm.state == PlayerMovement.MovementState.walking)
        {
            soundEffectSource.clip = Sprint;
            soundEffectSource.Play();
        }*/

/*        if (pd.hasDashed)
        {
            soundEffectSource.clip = Dash;
            soundEffectSource.Play();
        }*/
    }
}
