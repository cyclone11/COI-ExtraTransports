using ExtraTransports.Data;
using Mafi;
using Mafi.Collections;
using Mafi.Core.Mods;
namespace ExtraTransports
{
    public sealed class ExtraTransportsMod : DataOnlyMod
    {
        public ExtraTransportsMod(ModManifest manifest) : base(manifest)
        {
            Log.Info("[ExtraTransports] Constructed");
        }

        public override void RegisterPrototypes(ProtoRegistrator registrator)
        {
            registrator.RegisterData<TransportsData>();
            registrator.RegisterData<TransportsResearchData>();
        }

        public override void MigrateJsonConfig(VersionSlim savedVersion, Dict<string, object> savedValues)
        {
            // 模组当前没有用户配置，也没有需要迁移的配置数据。
        }
    }
}