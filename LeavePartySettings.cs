using System.Windows.Forms;
using PoeHUD.Hud.Settings;

namespace LeaveParty
{
    public class LeavePartySettings : SettingsBase
    {
        public HotkeyNode LeaveHotkey { get; set; } = Keys.F4;
    }
}
