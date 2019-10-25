using System;
using Audrey;
using Events;
using GameJam.Events.UI;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class SettingsDirector : BaseDirector
    {
        Root _root;

        bool isOnLeftSide = true;
        int leftSideIndex = 0;

        public SettingsDirector(Engine engine, ContentManager content, ProcessManager processManager, Root root) : base(engine, content, processManager)
        {
            this._root = root;
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<DisplaySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ControlsSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GameSettingsButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<SpeedSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<DifficultySettingsButtonPressedEvent>(this);
            // Plus all the rest of them
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            // Listen for the 4 types of button settings pressed
            // Consider buttonSelectedEvent and buttonDeselectedEvent to allow showing of right side
            DisplaySettingsButtonPressedEvent displaySBPE = evt as DisplaySettingsButtonPressedEvent;
            if (displaySBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 0;
                Console.WriteLine("displaySBPE");
                ((Button)_root.FindWidgetByID("Display")).isSelected = false;
                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("FullScreen")).isSelected = true;
            }
            ControlsSettingsButtonPressedEvent controlsSBPE = evt as ControlsSettingsButtonPressedEvent;
            if (controlsSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 1;
                Console.WriteLine("controlSBPE");
                ((Button)_root.FindWidgetByID("Controls")).isSelected = false;
                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("Rotate_Left")).isSelected = true;
            }
            GameSettingsButtonPressedEvent gameSBPE = evt as GameSettingsButtonPressedEvent;
            if (gameSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 2;
                Console.WriteLine("gameSBPE");
                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = false;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("Speed")).isSelected = true;
            }
            SpeedSettingsButtonPressedEvent speedSBPE = evt as SpeedSettingsButtonPressedEvent;
            if (speedSBPE != null)
            {
                Console.WriteLine("speedSBPE");
                // Save settings to the speed set by the player by modifying the cvars
            }
            DifficultySettingsButtonPressedEvent difficultySBPE = evt as DifficultySettingsButtonPressedEvent;
            if (difficultySBPE != null)
            {
                Console.WriteLine("difficultySBPE");
                // Save settings to the spawn rate set by the player by modifying the cvars
            }
            return false;
        }
    }
}
