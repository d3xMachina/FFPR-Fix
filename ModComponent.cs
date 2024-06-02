using System;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FFPR_Fix;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance { get; private set; }
    private bool _isDisabled;
    private float defaultTimeScale;
    public int DefaultFrameRate = 60;

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
            defaultTimeScale = Time.timeScale;

            Plugin.Log.LogInfo($"[{nameof(ModComponent)}].{nameof(Awake)}: Processed successfully.");
        }
        catch (Exception e)
        {
            _isDisabled = true;
            Plugin.Log.LogError($"[{nameof(ModComponent)}].{nameof(Awake)}(): {e}");
        }
    }

    public void Update()
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
            Plugin.Log.LogError($"[{nameof(ModComponent)}].{nameof(Update)}(): {e}");
        }
    }

    private void UpdateTimeScale()
    {
        float newTimeScale = defaultTimeScale;
        float newBattleSpeed = defaultTimeScale;

        var triggerL = Gamepad.current.leftTrigger.ReadValue();
        //Plugin.Log.LogInfo("Trigger value: " + triggerL);

        if (triggerL >= 0.90f || Input.GetKeyDown(KeyCode.T))
        {
            //Plugin.Log.LogInfo("Key down!");
            newTimeScale *= Plugin.Config.outBattleSpeedHackFactor.Value;
            newBattleSpeed *= Plugin.Config.battleSpeedHackFactor.Value;
        }

        if (Plugin.Config.outBattleSpeedHackFactor.Value != 1f && Time.timeScale != 0f)
        {
            Time.timeScale = newTimeScale;
        }

        if (Plugin.Config.battleSpeedHackFactor.Value != 1f)
        {
            var battlePlugManager = Last.Battle.BattlePlugManager.instance;
            if (battlePlugManager != null)
            {
                var battleOption = battlePlugManager.BattleOption;
                if (battleOption != null && battleOption.GetGameSpeed() != 0f)
                {
                    battleOption.SetGameSpeed(newBattleSpeed);
                }
            }
        }
    }
}
