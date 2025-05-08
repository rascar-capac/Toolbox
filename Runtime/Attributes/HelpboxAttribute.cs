using UnityEngine;

// TODO: replace with odin or naughty attributes
namespace Rascar.Toolbox.Attributes
{
    public sealed class HelpboxAttribute : PropertyAttribute
    {
        public string Text { get; }
        public EType Type { get; }

        public HelpboxAttribute(string text, EType messageType = EType.Info)
        {
            Text = text;
            Type = messageType;
        }

        public enum EType
        {
            None = 0,
            Info = 1,
            Warning = 2,
            Error = 3,
        }
    }
}
