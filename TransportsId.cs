using Mafi.Core.Entities.Static;
using Mafi.Core.Research;

namespace ExtraTransports
{
    // 已发布设施的 ID 会写入存档，0.8.8 按官方兼容规则继续保留旧名称。
    // 以后新增的原型应使用 ExtraTransports. 前缀。
    public static class TransportsId
    {

        public static class Transports
        {

            // Loose Material Transports

            public static readonly StaticEntityProto.ID LooseMaterialConveyorT4 = new StaticEntityProto.ID("LooseMaterialConveyorT4");

            public static readonly StaticEntityProto.ID LooseMaterialConveyorT5 = new StaticEntityProto.ID("LooseMaterialConveyorT5");

            // Unit Item Transports

            public static readonly StaticEntityProto.ID FlatConveyorT4 = new StaticEntityProto.ID("FlatConveyorT4");

            public static readonly StaticEntityProto.ID FlatConveyorT5 = new StaticEntityProto.ID("FlatConveyorT5");

            // Fluid Transpots

            public static readonly StaticEntityProto.ID FluidPipeT5 = new StaticEntityProto.ID("FluidPipeT5");
        }

        public static class Research
        {

            // Tier 4 Conveyor Research

            public static readonly ResearchNodeProto.ID ConveyorBeltsT4 =
                new ResearchNodeProto.ID("ConveyorBeltsT4");

            // Tier 5 Conveyor Research

            public static readonly ResearchNodeProto.ID ConveyorBeltsT5 =
                new ResearchNodeProto.ID("ConveyorBeltsT5");

            // 保留公开的旧 ID，当前五级管道随 ConveyorBeltsT5 一起解锁。

            public static readonly ResearchNodeProto.ID PipeT5 =
                new ResearchNodeProto.ID("PipeT5");
        }
    }
}
