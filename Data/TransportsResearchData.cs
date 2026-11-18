using Mafi;
using Mafi.Base.Prototypes.Research;
using Mafi.Core.Mods;
using Mafi.Core.Research;
using BaseIds = Mafi.Base.Ids;
using ModIds = ExtraTransports.TransportsId;

namespace ExtraTransports.Data
{
    internal sealed class TransportsResearchData : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            // 英文原文由 builder 注册，游戏从 Translations 目录读取玩家语言。
            var tier4 = registrator.ResearchNodeProtoBuilder
                .Start("Conveyor Belts IV", ModIds.Research.ConveyorBeltsT4,
                    ResearchCosts.ConveyorBeltsT3 * 2, "Title of the tier IV conveyor research.")
                .Description("Conveyor belts with increased throughput.",
                    "Unlocks tier IV flat and loose-material conveyors.")
                .SetGridPosition(new Vector2i(148, 14))
                .AddParents(new ResearchNodeProto[]
                {
                    registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(BaseIds.Research.ResearchLab4)
                });
            tier4.AddTransportToUnlock(ModIds.Transports.LooseMaterialConveyorT4);
            tier4.AddTransportToUnlock(ModIds.Transports.FlatConveyorT4);
            ResearchNodeProto tier4Node = tier4.BuildAndAdd();

            // 五级管道与输送带共用一项研究，沿用已发布的研究 ID。
            var tier5 = registrator.ResearchNodeProtoBuilder
                .Start("Conveyors and Pipes V", ModIds.Research.ConveyorBeltsT5,
                    ResearchCosts.ConveyorBeltsT3 * 3, "Title of the tier V transport research.")
                .Description("Increased throughput for conveyor belts and pipes.",
                    "Unlocks tier V flat conveyors, loose-material conveyors, and pipes.")
                .SetGridPosition(new Vector2i(168, 14))
                .AddParents(new ResearchNodeProto[] { tier4Node });
            tier5.AddTransportToUnlock(ModIds.Transports.LooseMaterialConveyorT5);
            tier5.AddTransportToUnlock(ModIds.Transports.FlatConveyorT5);
            tier5.AddTransportToUnlock(ModIds.Transports.FluidPipeT5);
            tier5.BuildAndAdd();
        }
    }
}
