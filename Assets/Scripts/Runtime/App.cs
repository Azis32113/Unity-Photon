using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [Header("UI Panel Configs")]
    [SerializeField] private UIPanel uiStart;
    [SerializeField] private UIPanel uiRoom;

    [Header("Button Configs")]
    [SerializeField] private Button btnHost;
    [SerializeField] private Button btnJoin;
    [SerializeField] private Button btnConnect;
    [SerializeField] private Button btnEnter;
    [SerializeField] private Button btnBack;

    [Header("Input Field Configs")]
    [SerializeField] private TMP_InputField inputField;

    private GameMode gameMode;

    private void Awake() {
        uiStart.Init();
        uiRoom.Init();
    }

    private void Start() {
        

        btnHost.onClick.AddListener(OnHostOption);
        btnJoin.onClick.AddListener(OnJoinOption);
        btnConnect.onClick.AddListener(OnSharedOption);
        btnBack.onClick.AddListener(OnBackToStart);
        btnEnter.onClick.AddListener(OnEnterRoom);
    }

    private void OnHostOption()
    {
        SetGameMode(GameMode.Host);
    }

    private void OnJoinOption()
    {
        SetGameMode(GameMode.Client);
    }

    private void OnSharedOption()
    {
        SetGameMode(GameMode.AutoHostOrClient);
    }

    private async void OnEnterRoom()
    {
        if (GateUI(uiRoom)) {
            await NetworkRunnerHandler.Instance.StartGame(gameMode);
        }
    }

    private void OnBackToStart()
    {
        if (GateUI(uiRoom)) uiStart.SetVisible(true);
    }

    private void SetGameMode(GameMode gamemode)
    {
        this.gameMode = gamemode;
        if (GateUI(uiStart)) uiRoom.SetVisible(true);

    }

    private bool GateUI(UIPanel uiPanel)
    {
        if (!uiPanel.IsShowing) return false;
            
        uiPanel.SetVisible(false);
        return true;
    }
}
