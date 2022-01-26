using UnityEngine;

public class PlayerHandUI : MonoBehaviour {
    public HandPopUpUI handPopUpUI;
    public EasyGameManager.GameStates[] showGameStates;
    protected virtual void Start() {
        EasyGameManager.instance.onGameStateChange += CheckGameState;
    }

    protected virtual void CheckGameState(EasyGameManager.GameStates gameState) {
        bool show = false;
        foreach (var showGameState in showGameStates) {
            if (gameState == showGameState) {
                show = true;
                break;
            }
        }
        handPopUpUI.SetShowCondition(show);
        if (!show)
            handPopUpUI.ActivateHolder(false);
    }

    protected virtual void OnDestroy() {
        EasyGameManager.instance.onGameStateChange -= CheckGameState;
    }
}