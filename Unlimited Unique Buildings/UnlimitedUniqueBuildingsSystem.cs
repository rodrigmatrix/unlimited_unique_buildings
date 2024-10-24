using Game.Prefabs;
using Game;
using Unity.Collections;
using Colossal.Serialization.Entities;
using Unity.Entities;
using System;
using Colossal.Entities;

namespace UnlimitedUniqueBuildings
{
    public partial class UnlimitedUniqueBuildingsSystem : GameSystemBase
    {
        
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            try
            {
                var query = GetEntityQuery(new EntityQueryDesc()
                {
                    All = [
                        ComponentType.ReadWrite<PlaceableObjectData>()
                    ]
                });
                var buildings = query.ToEntityArray(Allocator.Temp);

                foreach (var building in buildings)
                {
                    PlaceableObjectData data;

                    if (EntityManager.TryGetComponent(building, out data))
                    {
                        data.m_Flags &= ~Game.Objects.PlacementFlags.Unique;
                        EntityManager.SetComponentData(building, data);
                    }
                }

                buildings.Dispose();
            } catch(Exception e)
            {
                Mod.log.Warn($"Error updating Unlimited Unique Buildings " + e);
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}

