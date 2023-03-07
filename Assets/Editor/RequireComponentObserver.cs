using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UnityEngine
{
    [InitializeOnLoad]
    static class RequireComponentObserver
    {
        static RequireComponentObserver()
        {
            ObjectChangeEvents.changesPublished += (ref ObjectChangeEventStream stream) =>
            {
                for (int i = 0; i < stream.length; i++)
                {
                    switch (stream.GetEventType(i))
                    {
                        case ObjectChangeKind.ChangeGameObjectStructure:
                            stream.GetChangeGameObjectStructureEvent(i, out ChangeGameObjectStructureEventArgs args);
                            var go = EditorUtility.InstanceIDToObject(args.instanceId) as GameObject;
                            var components = go.GetComponents<Component>();
                            List<(string msg, Component cmp)> removeComponents = null;
                            foreach (var cmp in components)
                            {
                                foreach (var atr in cmp.GetType().GetCustomAttributes(false).OfType<IRequireComponentAttribute>())
                                {
                                    if (atr == null) continue;
                                    if (!atr.IsValid(components))
                                    {
                                        var msg = $"Removed [{cmp.GetType().Name}] by '{atr.GetType().Name}'\n{atr.Note}\n";
                                        removeComponents ??= new List<(string msg, Component cmp)>();
                                        removeComponents.Add((msg, cmp));
                                    }
                                }
                            }
                            if (removeComponents != null)
                            {
                                foreach (var pair in removeComponents)
                                {
                                    Debug.LogError(pair.msg);
                                    GameObject.DestroyImmediate(pair.cmp);
                                }
                                EditorUtility.SetDirty(go);
                            }
                            break;
                    }
                }
            };
        }
    }
}
