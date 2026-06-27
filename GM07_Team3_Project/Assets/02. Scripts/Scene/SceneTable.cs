using System.Collections.Generic;

/*
 * SceneTable
 * SceneTable 클래스는 외부에서 씬 타입을 통해 씬 로드를 할 수 있도록 하는 정적 클래스입니다.
 */
public static class SceneTable
{
    // 씬 타입과 씬 이름을 매핑하는 딕셔너리
    // 씬 타입 - 추가하고자하는 씬 타입을 SceneType enum에 추가해야함
    // 씬 이름 - 폴더 내에 존재하는 씬의 이름과 일치해야함
    private static readonly Dictionary<SceneType, string> sceneTable = new Dictionary<SceneType, string>()
    {
        {SceneType.MainMenu, "MainManuScene" },
        {SceneType.GameScene, "GamePlayScene" },
        {SceneType.GameOver, "GameOverSceneTest"}
    };

    // 씬 타입에 매핑되는 씬 이름을 반환하는 메서드
    public static string GetSceneName(SceneType sceneType)
    {
        // 파라미터로 받은 씬 타입이 실제로 딕셔너리에 존재하는지 확인하고 존재한다면 씬 이름을 반환
        if (sceneTable.TryGetValue(sceneType, out string sceneName))
        {
            return sceneName;
        }
        // 존재하지 않는 씬타입을 넣은 경우 에러 메시지를 출력
        else
        {
            throw new KeyNotFoundException($"Scene type {sceneType} not found in the scene table.");
        }
    }
}
