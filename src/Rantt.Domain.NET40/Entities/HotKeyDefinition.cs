// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HotKeyDefinition.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Entities
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Catel.Data;
    
    public class HotKeyDefinition : ModelBase
    {
        /// <summary>
        /// Unique action name to be referenced.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The key.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// The modifier.
        /// </summary>
        public ModifierKeys Modifier { get; set; }

        /// <summary>
        /// The short description of hot key.
        /// </summary>
        public string HotKeyDescription
        {
            get
            {

                var modifiers = new List<string>();

                if ((Modifier & ModifierKeys.Windows) == ModifierKeys.Windows)
                {
                    modifiers.Add("Win");
                }

                if ((Modifier & ModifierKeys.Alt) == ModifierKeys.Alt)
                {
                    modifiers.Add("Alt");
                }

                if ((Modifier & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    modifiers.Add("Ctrl");
                }

                if ((Modifier & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    modifiers.Add("Shift");
                }

                if (Key == Key.None)
                {
                    return string.Empty;
                }

                string prefix = (modifiers.Count > 0) ? string.Join(" + ", modifiers) + " + " : string.Empty;

                return prefix + Key.ToString();
            }
        }
    }
}
