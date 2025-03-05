using Il2CppSystem.Input;
using Il2CppSystem.Input.KeyConfig;
using System;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FFPR_Fix;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance { get; private set; }
    private bool _isDisabled;

    private float _lastGameTimeScale = 1f;
    private float _lastTimeScale = 1f;

    private bool _keySelectUp = true;

    public int DefaultFrameRate = 60;
    public int LastFrameRate = 60;
    public bool FixGetFrameRate = false;

    public Vector2 LastPadAxis = new();
    public Vector2 LastPadAxisSanitized = new();
    public bool ProcessPadNoInput = false;

    public BattleState CurrentBattleState = new();

    public ModComponent(IntPtr ptr) : base(ptr) { }

    public static bool Inject()
    {
        ClassInjector.RegisterTypeInIl2Cpp<ModComponent>();
        var name = typeof(ModComponent).FullName;

        Plugin.Log.LogInfo($"Initializing game object {name}");
        var modObject = new GameObject(name)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        GameObject.DontDestroyOnLoad(modObject);

        Plugin.Log.LogInfo($"Adding {name} to game object...");
        ModComponent component = modObject.AddComponent<ModComponent>();
        if (component == null)
        {
            GameObject.Destroy(modObject);
            Plugin.Log.LogError($"The game object is missing the required component: {name}");
            return false;
        }

        return true;
    }

    public void Awake()
    {
        try
        {
            Instance = this;
            _lastGameTimeScale = Time.timeScale;
            _lastTimeScale = _lastGameTimeScale;

            Plugin.Log.LogInfo($"[{nameof(ModComponent)}].{nameof(Awake)}: Processed successfully.");
        }
        catch (Exception e)
        {
            _isDisabled = true;
            Plugin.Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {e}");
        }
    }

    public void LateUpdate()
    {
        try
        {
            if (_isDisabled)
            {
                return;
            }

            UpdateTimeScale();
            UpdateBattle();
        }
        catch (Exception e)
        {
            _isDisabled = true;
            Plugin.Log.LogError($"[{nameof(ModComponent)}].{nameof(LateUpdate)}(): {e}");
        }
    }

    private void UpdateTimeScale()
    {
        var timeScale = Time.timeScale;
        if (timeScale == 0f)
        {
            // Game paused
            return;
        }

        var gameTimeScale = timeScale != _lastTimeScale ? timeScale : _lastGameTimeScale;
        var newTimeScale = gameTimeScale;

        var keyPageUp = InputListener.Instance?.GetKey(Il2CppSystem.Input.Key.PageUp, KeyValue.InputDeviceType.GamePad) ?? false;
        if (keyPageUp || Input.GetKey(KeyCode.T))
        {
            var isBattle = Last.Battle.BattlePlugManager.Instance()?.IsBattle() ?? false;
            var speedHackFactor = isBattle ? Plugin.Config.BattleSpeedHackFactor.Value : Plugin.Config.OutBattleSpeedHackFactor.Value;
            if (speedHackFactor > 0f)
            {
                newTimeScale *= speedHackFactor;
            }
        }

        _lastGameTimeScale = gameTimeScale;
        _lastTimeScale = newTimeScale;

        Time.timeScale = newTimeScale;
    }

    private void UpdateBattle()
    {
        var keySelect = Gamepad.current?.selectButton.isPressed ?? false; // select button is not mapped in the game
        keySelect = keySelect || Input.GetKey(KeyCode.P);

        if (!keySelect)
        {
            _keySelectUp = true;
        }
        else if (_keySelectUp)
        {
            CurrentBattleState.PassTurn();
            _keySelectUp = false;
        }
    }
}
