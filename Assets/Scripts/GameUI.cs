using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public void NextLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
