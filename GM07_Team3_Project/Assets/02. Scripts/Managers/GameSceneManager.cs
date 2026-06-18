using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * GameSceneManager
 * 씬 매니저는 게임의 씬을 전환하거나 리로딩하는 기능을 담당하는 매니저입니다.
 * 이 때 씬 매니저는 미리 정의한 씬 타입을 기반으로 기능을 수행합니다.
 */
public class GameSceneManager : Singleton<GameSceneManager>
{
    // 

    // enum으로 씬 타입을 파라미터로 받아 타입 기반의 씬 로드를 작동
    public void LoadScene(SceneType type)
    {
       string sceneName = SceneTable.GetSceneName(type);
       SceneManager.LoadScene(sceneName);
    }

    // 씬을 리로드하는 기능, 현재 씬을 다시 로드하여 초기 상태로 되돌리는 기능을 수행
    // 예를 들어 게임 오버 후 Retry 버튼을 눌렀을 때 게임 씬을 리로드하여 다시 게임을 시작할 수 있도록 함.
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
