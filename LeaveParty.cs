using System;
using System.Windows.Forms;
using PoeHUD.Hud.UI;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.RemoteMemoryObjects;
using Stashie.Utils;

namespace LeaveParty
{
    public class LeaveParty : BaseSettingsPlugin<LeavePartySettings>
    {
        private IngameState _ingameState;

        public override void Initialise()
        {
            _ingameState = GameController.Game.IngameState;
        }

        public override void DrawSettingsMenu()
        {
            Settings.LeaveHotkey.Value = ImGuiExtension.HotkeySelector("Leave Party Hotkey", Settings.LeaveHotkey.Value);
        }

        public override void Render()
        {
            if (!Settings.Enable)
                return;

            if (!Keyboard.IsKeyToggled(Settings.LeaveHotkey.Value))
                return;
            
            LeaveCurrentParty();

            Keyboard.KeyPress(Settings.LeaveHotkey.Value);
        }

        private void LeaveCurrentParty()
        {
            if (!IsInParty())
                return;

            var socialPanel = _ingameState.UIRoot.Children[1].Children[16].Children[2].Children[0].Children[5];
            if(!socialPanel.IsVisible)
                Keyboard.KeyPress(Keys.S);

            var windowOffset = GameController.Window.GetWindowRectangle().TopLeft;
            var partyPanel = socialPanel.Children[1].Children[2];
            if (!partyPanel.IsVisible)
            {
                var partyTab = FindElement(socialPanel.Children[0], element => element.AsObject<EntityLabel>().Text.Equals("Current Party", StringComparison.OrdinalIgnoreCase));
                Mouse.SetCursorPosAndLeftClick(partyTab.GetClientRect().Center, 50, windowOffset);
            }

            var leaveButton = FindElement(partyPanel, (element) => element.AsObject<EntityLabel>().Text == "leave");
            if (leaveButton == null)
                return;

            Mouse.SetCursorPosAndLeftClick(leaveButton.GetClientRect().Center, 50, windowOffset);
            Keyboard.KeyPress(Keys.S);
        }

        private Element FindElement(Element rootElement, Func<Element, bool> condition)
        {
            foreach (var child in rootElement.Children)
            {
                if (condition(child))
                    return child;

                var result = FindElement(child, condition);
                if (result != null)
                    return result;
            }

            return null;
        }

        private bool IsInParty()
        {
            var partyElement = GameController.Game.IngameState.UIRoot.Children[1].Children[7].Children[0].Children[0];
            return partyElement.ChildCount > 0;
        }
    }
}
