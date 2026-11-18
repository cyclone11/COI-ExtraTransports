using Mafi;
using Mafi.Base.Prototypes.Research;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using Mafi.Localization;
using BaseIds = Mafi.Base.Ids;
using ModIds = ExtraTransports.TransportsId;

namespace ExtraTransports.Data
{
    internal sealed class TransportsResearchData : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            var tier4 = new LocalizedResearch(registrator.ResearchNodeProtoBuilder, ModIds.Research.ConveyorBeltsT4,
                    ExtraTransportsTexts.ConveyorResearchT4Name, ResearchCosts.ConveyorBeltsT3 * 2)
                .Description(ExtraTransportsTexts.ConveyorResearchT4Description)
                .SetGridPosition(new Vector2i(120, 15))
                .AddParents(new ResearchNodeProto[]
                {
                    registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(BaseIds.Research.ResearchLab4)
                });
            tier4.AddTransportToUnlock(ModIds.Transports.LooseMaterialConveyorT4);
            tier4.AddTransportToUnlock(ModIds.Transports.FlatConveyorT4);
            tier4.AddTransportToUnlock(ModIds.Transports.FluidPipeT5);
            ResearchNodeProto tier4Node = tier4.BuildAndAdd();

            var tier5 = new LocalizedResearch(registrator.ResearchNodeProtoBuilder, ModIds.Research.ConveyorBeltsT5,
                    ExtraTransportsTexts.ConveyorResearchT5Name, ResearchCosts.ConveyorBeltsT3 * 3)
                .Description(ExtraTransportsTexts.ConveyorResearchT5Description)
                .SetGridPosition(new Vector2i(124, 13))
                .AddParents(new ResearchNodeProto[] { tier4Node });
            tier5.AddTransportToUnlock(ModIds.Transports.LooseMaterialConveyorT5);
            tier5.AddTransportToUnlock(ModIds.Transports.FlatConveyorT5);
            tier5.BuildAndAdd();
        }

        private sealed class LocalizedResearch : ResearchNodeProtoBuilder.State
        {
            public LocalizedResearch(ResearchNodeProtoBuilder builder, ResearchNodeProto.ID id, LocStr name, int cost)
                : base(builder, id)
            {
                // Keep the save ID while retaining a live, mod-owned translation key.
                Name = name;
                SetCosts(cost);
            }
        }
    }
}
