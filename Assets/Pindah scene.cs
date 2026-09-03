using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    public string namaSceneTujuan;

    public void PindahKeScene()
    {
        if (!string.IsNullOrEmpty(namaSceneTujuan))
        {
            SceneManager.LoadScene(namaSceneTujuan);
        }
        else
        {
            Debug.LogError("Nama Scene Tujuan belum diisi di Inspector!");
        }
    }
}
