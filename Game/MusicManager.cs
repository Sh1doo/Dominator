using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    //音源とIDの対応付け
    public enum Music
    {
        SE_Wood,
    }

    [SerializeField] GameObject SE;
    [SerializeField] AudioClip[] audioClips;

    //SEを再生
    public void PlaySE(Music id)
    {
        AudioSource audioSource = SE.AddComponent<AudioSource>();
        audioSource.clip = audioClips[(int)id];
        StartCoroutine(PlayCoroutine(audioSource));
    }

    //終了すればコンポーネントを削除する
    private IEnumerator PlayCoroutine(AudioSource audioSource)
    {
        audioSource.Play();

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(audioSource);
    }

}
