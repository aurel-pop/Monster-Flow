using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] soundClips;

    private AudioSource audioSource;
    public float lowPitchRange;
    public float highPitchRange;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int i)
    {
        audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        audioSource.PlayOneShot(soundClips[i]);
    }
}
