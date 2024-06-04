using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FFPR_Fix;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance { get; private set; }
    private bool _isDisabled;

    private float _lastGameTimeScale = 1f;
    private float _lastTimeScale = 1f;

    public int DefaultFrameRate = 60;

    public Vector2 LastPadAxis = new();
    public Vector2 LastPadAxisSanitized = new();
    public bool ProcessPadNoInput = false;

    public ModComponent(IntPtr ptr) : base(ptr) { }

    public static void Inject()
    {
        ClassInjector.RegisterTypeInIl2Cpp<ModComponent>();
        var name = typeof(ModComponent).FullName;

        Plugin.Log.LogInfo($"Initializing game object {name}");
        var modObject = new GameObject(name);
        modObject.hideFlags = HideFlags.HideAndDontSave;
        GameObject.DontDestroyOnLoad(modObject);

        Plugin.Log.LogInfo($"Adding {name} to game object...");
        ModComponent component = modObject.AddComponent<ModComponent>();
        if (component == null)
        {
            GameObject.Destroy(modObject);
            Plugin.Log.LogError($"The game object is missing the required component: {name}");
        }
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

        var leftTrigger = Gamepad.current.leftTrigger.ReadValue();
        if (leftTrigger >= 0.90f || Input.GetKeyDown(KeyCode.T))
        {
            var isBattle = Last.Battle.BattlePlugManager.instance?.IsBattle() ?? false;
            var speedHackFactor = isBattle ? Plugin.Config.battleSpeedHackFactor.Value : Plugin.Config.outBattleSpeedHackFactor.Value;
            if (speedHackFactor > 0f)
            {
                newTimeScale *= speedHackFactor;
            }
        }

        _lastGameTimeScale = gameTimeScale;
        _lastTimeScale = newTimeScale;

        Time.timeScale = newTimeScale;
    }
}
