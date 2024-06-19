using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
  private AudioSource _audioSource;

  private void Awake()
  {
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  private void OnEnable()
  {
    _audioSource.Play();
  }

  private void OnDisable()
  {
    _audioSource.Stop();
  }
}