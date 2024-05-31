using Il2CppSystem.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FFPR_Fix
{
    public sealed class ModComponent : MonoBehaviour
    {
        public static ModComponent Instance { get; private set; }
        private bool _isDisabled;
        private float defaultTimeScale;

        public ModComponent(IntPtr ptr) : base(ptr) { }

        public void Awake()
        {
            try
            {
                Instance = this;
                defaultTimeScale = Time.timeScale;

                string msg = $"[{nameof(ModComponent)}].{nameof(Awake)}: Processed successfully.";
                Plugin.Log.LogInfo(msg);
            }
            catch (Exception e)
            {
                _isDisabled = true;
                string msg = $"[{nameof(ModComponent)}].{nameof(Awake)}(): {e}";
                Plugin.Log.LogError(msg);
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
                newTimeScale *= Plugin.outBattleSpeedHackFactor.Value;
                newBattleSpeed *= Plugin.battleSpeedHackFactor.Value;
            }

            if (Plugin.outBattleSpeedHackFactor.Value != 1f && Time.timeScale != 0f)
            {
                Time.timeScale = newTimeScale;
            }

            if (Plugin.battleSpeedHackFactor.Value != 1f)
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
                string msg = $"[{nameof(ModComponent)}].{nameof(Update)}(): {e}";
                Plugin.Log.LogError(msg);
            }
        }
    }
}
