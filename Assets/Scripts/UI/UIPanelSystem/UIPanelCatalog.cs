using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UIPanelCatalog : MonoBehaviour
{
    [SerializeField] AbstractUIPanel[] _panelBehaviours; // arrastra aquí tus paneles
    Dictionary<string, IUIPanel> _panels;

    public void Initialize()
    {
        _panels = _panelBehaviours
            .OfType<IUIPanel>()
            .ToDictionary(p => p.Id, p => p);
    }

    public bool TryGet(string id, out IUIPanel panel) => _panels.TryGetValue(id, out panel);
    public IEnumerable<IUIPanel> AllPanels() => _panels.Values;
}
