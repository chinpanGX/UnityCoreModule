using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    private async void Start()
    {
        await SceneManager.LoadSceneAsync("CoreScene", LoadSceneMode.Additive);
    }
}