using System.Collections.Generic;

using Game.Prefabs;
using Game;
using Unity.Collections;
using Colossal.Serialization.Entities;
using Unity.Entities;
using Colossal.Logging;
using System;

namespace UnlimitedUniqueBuildings
{
    public partial class UnlimitedUniqueBuildingsSystem : GameSystemBase
    {
        private Dictionary<Entity, PlaceableObjectData> _placebleData = new Dictionary<Entity, PlaceableObjectData>();

        private EntityQuery _query;

        public static ILog log = LogManager.GetLogger($"{nameof(UnlimitedUniqueBuildings)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            try
            {
                _query = GetEntityQuery(new EntityQueryDesc()
                {
                    All = [
                        ComponentType.ReadWrite<PlaceableObjectData>()
                    ]
                });
                var buildings = _query.ToEntityArray(Allocator.Temp);

                foreach (var building in buildings)
                {
                    PlaceableObjectData data;

                    if (!_placebleData.TryGetValue(building, out data))
                    {
                        data = EntityManager.GetComponentData<PlaceableObjectData>(building);
                        _placebleData.Add(building, data);
                    }
                    data.m_Flags &= ~Game.Objects.PlacementFlags.Unique;
                    EntityManager.SetComponentData(building, data);
                }
            } catch(Exception e)
            {
                log.Warn($"Error updating Unlimited Unique Buildings " + e);
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}

