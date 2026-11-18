using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;

namespace ExtraTransports.Data
{
    internal sealed class TransportsData : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            // 按等级注册，确保后续 GetOrThrow 能取得上一等级的原型。
            RegisterFlatConveyorT4(registrator);
            RegisterFlatConveyorT5(registrator);
            RegisterLooseMaterialConveyorT4(registrator);
            RegisterLooseMaterialConveyorT5(registrator);
            RegisterFluidPipeT5(registrator);
            Log.Info("[ExtraTransports] Registered four conveyors and one pipe.");
        }

        private static void RegisterFlatConveyorT4(ProtoRegistrator registrator)
        {
            // 从原版三级平面输送带继承几何和通行规则。
            ProtosDb prototypes = registrator.PrototypesDb;
            TransportProto source = prototypes.GetOrThrow<TransportProto>(Ids.Transports.FlatConveyorT3);
            IoPortShapeProto portShape = prototypes.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.FlatConveyor);
            ImmutableArray<ToolbarEntryData> categories = registrator.GetCategoriesProtos(
                new Proto.ID[] { Ids.ToolbarCategories.Transports_Flat });

            var transport = new TransportProto(
                TransportsId.Transports.FlatConveyorT4,
                new Proto.Str(ExtraTransportsTexts.FlatConveyorT4Name, ExtraTransportsTexts.FlatConveyorT4Description),
                source.SurfaceRelativeHeight,
                3.Quantity(),
                0.58.Tiles(),
                RelTile1f.FromTilesPerSecond(2.9),
                source.ZStepLength,
                source.NeedsPillarsAtGround,
                source.CanBeBuried,
                source.TileSurfaceWhenOnGround,
                source.MaxPillarSupportRadius,
                portShape,
                new Electricity(((float)Costs.Transports.ConveyorT3ElectricityPer100.Value * 1.5f)
                    .ToFix32().ToIntCeiled()),
                source.CornersSharpnessPercent,
                source.AllowMixedProducts,
                source.IsBuildable,
                TransportsCost.FlatConveyorT4.MapToEntityCosts(registrator),
                source.ConstructionDurationPerProduct,
                CreateGraphics(source, categories, "FlatConveyorT4"));

            prototypes.Add<TransportProto>(transport, false);
            source.SetNextTier(transport, true);
        }

        private static void RegisterFlatConveyorT5(ProtoRegistrator registrator)
        {
            // 五级耗电以原版三级基准计算，避免在四级倍率上再次相乘。
            ProtosDb prototypes = registrator.PrototypesDb;
            TransportProto source = prototypes.GetOrThrow<TransportProto>(TransportsId.Transports.FlatConveyorT4);
            IoPortShapeProto portShape = prototypes.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.FlatConveyor);
            ImmutableArray<ToolbarEntryData> categories = registrator.GetCategoriesProtos(
                new Proto.ID[] { Ids.ToolbarCategories.Transports_Flat });

            var transport = new TransportProto(
                TransportsId.Transports.FlatConveyorT5,
                new Proto.Str(ExtraTransportsTexts.FlatConveyorT5Name, ExtraTransportsTexts.FlatConveyorT5Description),
                source.SurfaceRelativeHeight,
                3.Quantity(),
                0.58.Tiles(),
                RelTile1f.FromTilesPerSecond(5.8),
                source.ZStepLength,
                source.NeedsPillarsAtGround,
                source.CanBeBuried,
                source.TileSurfaceWhenOnGround,
                source.MaxPillarSupportRadius,
                portShape,
                new Electricity(((float)Costs.Transports.ConveyorT3ElectricityPer100.Value * 2.25f)
                    .ToFix32().ToIntCeiled()),
                source.CornersSharpnessPercent,
                source.AllowMixedProducts,
                source.IsBuildable,
                TransportsCost.FlatConveyorT5.MapToEntityCosts(registrator),
                source.ConstructionDurationPerProduct,
                CreateGraphics(source, categories, "FlatConveyorT5"));

            prototypes.Add<TransportProto>(transport, false);
            source.SetNextTier(transport, true);
        }

        private static void RegisterLooseMaterialConveyorT4(ProtoRegistrator registrator)
        {
            // 散料带沿用原版的接口形状和支柱规则。
            ProtosDb prototypes = registrator.PrototypesDb;
            TransportProto source = prototypes.GetOrThrow<TransportProto>(Ids.Transports.LooseMaterialConveyorT3);
            IoPortShapeProto portShape = prototypes.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.LooseMaterialConveyor);
            ImmutableArray<ToolbarEntryData> categories = registrator.GetCategoriesProtos(
                new Proto.ID[] { Ids.ToolbarCategories.Transports_Loose });

            var transport = new TransportProto(
                TransportsId.Transports.LooseMaterialConveyorT4,
                new Proto.Str(ExtraTransportsTexts.LooseConveyorT4Name, ExtraTransportsTexts.LooseConveyorT4Description),
                source.SurfaceRelativeHeight,
                3.Quantity(),
                0.58.Tiles(),
                RelTile1f.FromTilesPerSecond(2.9),
                source.ZStepLength,
                source.NeedsPillarsAtGround,
                source.CanBeBuried,
                source.TileSurfaceWhenOnGround,
                source.MaxPillarSupportRadius,
                portShape,
                new Electricity(((float)Costs.Transports.ConveyorT3ElectricityPer100.Value * 1.5f)
                    .ToFix32().ToIntCeiled()),
                source.CornersSharpnessPercent,
                source.AllowMixedProducts,
                source.IsBuildable,
                TransportsCost.LooseMaterialConveyorT4.MapToEntityCosts(registrator),
                source.ConstructionDurationPerProduct,
                CreateGraphics(source, categories, "LooseMaterialConveyorT4"));

            prototypes.Add<TransportProto>(transport, false);
            source.SetNextTier(transport, true);
        }

        private static void RegisterLooseMaterialConveyorT5(ProtoRegistrator registrator)
        {
            // 先注册四级，再把五级接到四级升级链后。
            ProtosDb prototypes = registrator.PrototypesDb;
            TransportProto source = prototypes.GetOrThrow<TransportProto>(TransportsId.Transports.LooseMaterialConveyorT4);
            IoPortShapeProto portShape = prototypes.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.LooseMaterialConveyor);
            ImmutableArray<ToolbarEntryData> categories = registrator.GetCategoriesProtos(
                new Proto.ID[] { Ids.ToolbarCategories.Transports_Loose });

            var transport = new TransportProto(
                TransportsId.Transports.LooseMaterialConveyorT5,
                new Proto.Str(ExtraTransportsTexts.LooseConveyorT5Name, ExtraTransportsTexts.LooseConveyorT5Description),
                source.SurfaceRelativeHeight,
                3.Quantity(),
                0.58.Tiles(),
                RelTile1f.FromTilesPerSecond(5.8),
                source.ZStepLength,
                source.NeedsPillarsAtGround,
                source.CanBeBuried,
                source.TileSurfaceWhenOnGround,
                source.MaxPillarSupportRadius,
                portShape,
                new Electricity(((float)Costs.Transports.ConveyorT3ElectricityPer100.Value * 2.25f)
                    .ToFix32().ToIntCeiled()),
                source.CornersSharpnessPercent,
                source.AllowMixedProducts,
                source.IsBuildable,
                TransportsCost.LooseMaterialConveyorT5.MapToEntityCosts(registrator),
                source.ConstructionDurationPerProduct,
                CreateGraphics(source, categories, "LooseMaterialConveyorT5"));

            prototypes.Add<TransportProto>(transport, false);
            source.SetNextTier(transport, true);
        }

        private static void RegisterFluidPipeT5(ProtoRegistrator registrator)
        {
            // 五级管道复用四级管道的几何和输送规则，使用独立的绿色标记贴图。
            ProtosDb prototypes = registrator.PrototypesDb;
            TransportProto source = prototypes.GetOrThrow<TransportProto>(Ids.Transports.PipeT4);
            IoPortShapeProto portShape = prototypes.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.Pipe);
            ImmutableArray<ToolbarEntryData> categories = registrator.GetCategoriesProtos(
                new Proto.ID[] { Ids.ToolbarCategories.Transports_Fluid });

            var transport = new TransportProto(
                TransportsId.Transports.FluidPipeT5,
                new Proto.Str(ExtraTransportsTexts.PipeT5Name, ExtraTransportsTexts.PipeT5Description),
                source.SurfaceRelativeHeight,
                3.Quantity(),
                0.58.Tiles(),
                RelTile1f.FromTilesPerSecond(5.8),
                source.ZStepLength,
                source.NeedsPillarsAtGround,
                source.CanBeBuried,
                source.TileSurfaceWhenOnGround,
                source.MaxPillarSupportRadius,
                portShape,
                Electricity.Zero,
                source.CornersSharpnessPercent,
                source.AllowMixedProducts,
                source.IsBuildable,
                TransportsCost.PipeT5.MapToEntityCosts(registrator),
                source.ConstructionDurationPerProduct,
                CreateGraphics(source, categories, "FluidPipeT5"));

            prototypes.Add<TransportProto>(transport, false);
            source.SetNextTier(transport, true);
        }

        private static TransportProto.Gfx CreateGraphics(TransportProto source,
            ImmutableArray<ToolbarEntryData> categories, string id)
        {
            // EarlyInit 注册独立材质和内嵌菜单图标。
            // 每个原型分配独立的渲染缓存。
            return new TransportProto.Gfx(
                source.Graphics.CrossSectionLods,
                source.Graphics.RenderProducts,
                TransportVisuals.MaterialPath(id),
                source.Graphics.TransportUvLength,
                source.Graphics.RenderTransportedProducts,
                // 0.8.8 的声音来自游戏绑定表，此旧版构造参数不再使用。
                string.Empty,
                source.Graphics.FlowIndicator,
                source.Graphics.VerticalConnectorPrefabPath,
                source.Graphics.PillarAttachments,
                source.Graphics.UvShiftY,
                new TransportProto.Gfx.TransportInstancedRenderingData(),
                source.Graphics.CrossSectionRadius,
                source.Graphics.CrossSectionScale,
                source.Graphics.UsePerProductColoring,
                TransportVisuals.IconPath(id),
                true,
                source.Graphics.MaxRenderedLod,
                new ImmutableArray<ToolbarEntryData>?(categories),
                false);
        }
    }
}
