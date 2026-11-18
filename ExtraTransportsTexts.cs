using Mafi.Localization;

namespace ExtraTransports
{
    // 静态只读词条在模组加载时注册，供原生翻译和导出工具读取。
    internal static class ExtraTransportsTexts
    {
        public static readonly LocStr FlatConveyorT4Name =
            Loc.Str("ExtraTransports.FlatConveyorT4__name",
                "Flat Conveyor IV",
                "Name of the tier IV conveyor for unit products.");

        public static readonly LocStr FlatConveyorT4Description =
            Loc.Str("ExtraTransports.FlatConveyorT4__desc",
                "Conveys unit products or finished goods. Can carry multiple product types simultaneously.",
                "Description of the tier IV flat conveyor.");

        public static readonly LocStr FlatConveyorT5Name =
            Loc.Str("ExtraTransports.FlatConveyorT5__name",
                "Flat Conveyor V",
                "Name of the tier V conveyor for unit products.");

        public static readonly LocStr FlatConveyorT5Description =
            Loc.Str("ExtraTransports.FlatConveyorT5__desc",
                "Conveys unit products or finished goods. Can carry multiple product types simultaneously.",
                "Description of the tier V flat conveyor.");

        public static readonly LocStr LooseConveyorT4Name =
            Loc.Str("ExtraTransports.LooseMaterialConveyorT4__name",
                "Loose Conveyor IV",
                "Name of the tier IV conveyor for loose materials.");

        public static readonly LocStr LooseConveyorT4Description =
            Loc.Str("ExtraTransports.LooseMaterialConveyorT4__desc",
                "Transports loose materials. Can carry multiple product types simultaneously.",
                "Description of the tier IV loose-material conveyor.");

        public static readonly LocStr LooseConveyorT5Name =
            Loc.Str("ExtraTransports.LooseMaterialConveyorT5__name",
                "Loose Conveyor V",
                "Name of the tier V conveyor for loose materials.");

        public static readonly LocStr LooseConveyorT5Description =
            Loc.Str("ExtraTransports.LooseMaterialConveyorT5__desc",
                "Transports loose materials. Can carry multiple product types simultaneously.",
                "Description of the tier V loose-material conveyor.");

        public static readonly LocStr PipeT5Name =
            Loc.Str("ExtraTransports.FluidPipeT5__name",
                "Pipe V",
                "Name of the tier V fluid pipe.");

        public static readonly LocStr PipeT5Description =
            Loc.Str("ExtraTransports.FluidPipeT5__desc",
                "Transports liquids and gases. Each pipe segment can transport only one product type at a time.",
                "Description of the tier V fluid pipe.");

        public static readonly LocStr ConveyorResearchT4Name =
            Loc.Str("ExtraTransports.ConveyorBeltsT4__name",
                "Conveyor Belts IV and Pipes V", "Title of the tier IV conveyor and tier V pipe research.");

        public static readonly LocStr ConveyorResearchT4Description =
            Loc.Str("ExtraTransports.ConveyorBeltsT4__desc",
                "Unlocks tier IV flat and loose-material conveyors and tier V pipes.",
                "Description of the tier IV conveyor and tier V pipe research.");

        public static readonly LocStr ConveyorResearchT5Name =
            Loc.Str("ExtraTransports.ConveyorBeltsT5__name",
                "Conveyor Belts V", "Title of the tier V conveyor research.");

        public static readonly LocStr ConveyorResearchT5Description =
            Loc.Str("ExtraTransports.ConveyorBeltsT5__desc",
                "Unlocks tier V flat and loose-material conveyors.",
                "Description of the tier V conveyor research.");
    }
}
