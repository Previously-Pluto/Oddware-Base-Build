using System.Collections.Generic;
using System.Linq;
using Naninovel;
using PixelCrushers;
using RoomManagement;
using SlimUI.CursorControllerPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Serialization;

[CommandAlias("toggleHotSpots")]
public class ToggleHotSpots : Command
{
    /// <summary>
    /// IDs of the actors to hide.
    /// </summary>
    [ParameterAlias(NamelessParameterAlias), RequiredParameter, ActorContext]
    public StringListParameter ActorIds;

    /// <summary>
    /// Whether to remove (destroy) the actor after it's hidden.
    /// Use to unload resources associated with the actor and prevent memory leaks.
    /// </summary>
    [ParameterDefaultValue("true")]
    public BooleanParameter Toggle = false;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var managers = Engine.FindAllServices<IActorManager>(c => ActorIds.Any(id => c.ActorExists(id)));
        var tasks = new List<UniTask>();
        foreach (var actorId in ActorIds)
            if (managers.FirstOrDefault(m => m.ActorExists(actorId)) is IActorManager manager)
                tasks.Add(ToggleInManager(actorId, manager, Toggle));
            else Err($"Failed to hide `{actorId}` actor: can't find any managers with `{actorId}` actor.");
        await UniTask.WhenAll(tasks);
    }

    private async UniTask ToggleInManager(string actorId, IActorManager manager, bool toggle)
    {
        var actor = manager.GetActor(actorId);
        LayeredBackground actorBackground = actor as LayeredBackground;
        var actorBehavior = actorBackground.Behaviour;
        if (actorBehavior != null)
        {
            var hotSpots = actorBehavior.GetComponentsInChildren<HotSpot>();
            foreach (var hotSpot in hotSpots)
            {
                hotSpot.ToggleHotSpot(toggle);
            }

            // for (int i = 0; i < childCount; i++)
            // {
            //     //actorBehavior.transform.GetChild(i).gameObject.SetActive(false);
            // }
        }

        CursorController.Instance.tooltipController.HideTooltip();
        ContextMenuManager.Instance.ClearContextItem();
    }
}