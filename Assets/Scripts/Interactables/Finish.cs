using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : Interactable
{
    private readonly float waitTime = 2f;
    private AudioSource winSound;
    private ParticleSystem particleSystem;

    private void Awake() {
        winSound = GetComponent<AudioSource>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public override void Interact(Player player)
    {
        winSound.Play();
        particleSystem.Play();
        StartCoroutine(RestartWithDelay());
    }

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
