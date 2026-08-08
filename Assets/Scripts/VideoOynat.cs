using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoOynat : MonoBehaviour
{
    public VideoClip videoClip;
    public Button cikisButonu;

    void Start()
    {
        // ✅ FAREYİ GÖSTER
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        VideoPlayer vp = gameObject.AddComponent<VideoPlayer>();
        RenderTexture rt = new RenderTexture(1920, 1080, 0);
        vp.clip = videoClip;
        vp.targetTexture = rt;
        GetComponent<RawImage>().texture = rt;
        vp.Play();

        cikisButonu.onClick.AddListener(AnaMenuyeDon);
    }

    void AnaMenuyeDon()
    {
        SceneManager.LoadScene("MainMenu");
    }
}