using System.Collections.Generic;

using Game.Prefabs;
using Game;
using Unity.Collections;
using Unity.Entities;

namespace UnlimitedUniqueBuildings
{
    public partial class UnlimitedUniqueBuildingsSystem : GameSystemBase
    {
        private Dictionary<Entity, PlaceableObjectData> _placebleData = new Dictionary<Entity, PlaceableObjectData>();

        private EntityQuery _query;

        protected override void OnCreate()
        {
            base.OnCreate();

            _query = GetEntityQuery(new EntityQueryDesc()
            {
                All = [
                    ComponentType.ReadWrite<PlaceableObjectData>()
                ]
            });

            RequireForUpdate(_query);
        }

        protected override void OnUpdate()
        {
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
        }
    }
}

