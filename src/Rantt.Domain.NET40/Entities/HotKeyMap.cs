namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Catel.Data;

    #if (!SILVERLIGHT)
    [Serializable]
    public class HotKeyMap : SavableModelBase<HotKeyMap>
    #else
    public class HotKeyMap
#endif
    {
        public HotKeyMap()
        {
            HotKeys = new ObservableCollection<HotKeyDefinition>();
        }

        public ObservableCollection<HotKeyDefinition> HotKeys { get; set; }
    }
}
