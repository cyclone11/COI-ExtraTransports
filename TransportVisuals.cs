using System;
using System.Collections.Generic;
using System.IO;
using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Unity;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ExtraTransports
{
    internal sealed class TransportVisuals : IDisposable
    {
        private delegate bool LoadImageBytes(Texture2D texture, byte[] bytes, bool markNonReadable);

        // Unity 6 also exposes a Span overload that net48 reference assemblies cannot
        // resolve. Bind the supported byte-array overload explicitly.
        private static readonly LoadImageBytes s_loadImage = (LoadImageBytes)Delegate.CreateDelegate(
            typeof(LoadImageBytes), typeof(ImageConversion).GetMethod("LoadImage",
                new[] { typeof(Texture2D), typeof(byte[]), typeof(bool) })!);

        private readonly AssetsDb m_assets;
        private readonly List<UnityObject> m_ownedObjects = new List<UnityObject>();
        private readonly Dictionary<string, UnityObject> m_overrides = new Dictionary<string, UnityObject>();

        private TransportVisuals(AssetsDb assets)
        {
            m_assets = assets;
        }

        public static string MaterialPath(string id) => $"Assets/ExtraTransports/Materials/{id}.mat";

        public static string IconPath(string id) => $"Assets/ExtraTransports/Icons/{id}.png";

        public static TransportVisuals Install(DependencyResolver resolver)
        {
            var visuals = new TransportVisuals(resolver.Resolve<AssetsDb>());
            try
            {
                var prototypes = resolver.Resolve<ProtosDb>();
                visuals.RegisterTransport(prototypes, Ids.Transports.FlatConveyorT3, "FlatConveyorT4");
                visuals.RegisterTransport(prototypes, Ids.Transports.FlatConveyorT3, "FlatConveyorT5");
                visuals.RegisterTransport(prototypes, Ids.Transports.LooseMaterialConveyorT3, "LooseMaterialConveyorT4");
                visuals.RegisterTransport(prototypes, Ids.Transports.LooseMaterialConveyorT3, "LooseMaterialConveyorT5");
                visuals.RegisterTransport(prototypes, Ids.Transports.PipeT4, "FluidPipeT5");
                Log.Info("[ExtraTransports] Loaded purple tier IV / green tier V materials and five menu icons.");
                return visuals;
            }
            catch
            {
                visuals.Dispose();
                throw;
            }
        }

        private void RegisterTransport(ProtosDb prototypes, StaticEntityProto.ID sourceId, string id)
        {
            var source = prototypes.GetOrThrow<TransportProto>(sourceId);
            var originalMaterial = m_assets.GetSharedMaterial(source.Graphics.MaterialPath);
            var material = new Material(originalMaterial) { name = $"ExtraTransports.{id}" };
            m_ownedObjects.Add(material);

            // Replace the albedo only. The cloned material retains the original shader,
            // normal map, metallic map, and conveyor animation settings.
            Texture2D albedo = LoadPng($"Textures.{id}.png", true);
            Texture originalAlbedo = originalMaterial.mainTexture;
            albedo.filterMode = originalAlbedo.filterMode;
            albedo.wrapMode = originalAlbedo.wrapMode;
            albedo.anisoLevel = originalAlbedo.anisoLevel;
            material.mainTexture = albedo;
            RegisterOverride(MaterialPath(id), material);
            RegisterIcon(id);
        }

        private void RegisterIcon(string id)
        {
            Texture2D icon = LoadPng($"Icons.{id}.png", false);
            icon.filterMode = FilterMode.Bilinear;
            icon.wrapMode = TextureWrapMode.Clamp;
            RegisterOverride(IconPath(id), icon);
        }

        private Texture2D LoadPng(string resource, bool mipmaps)
        {
            string resourceName = "ExtraTransports.Assets." + resource;
            using (Stream stream = typeof(TransportVisuals).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Missing embedded visual asset: {resourceName}"))
            using (var buffer = new MemoryStream())
            {
                stream.CopyTo(buffer);
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipmaps, false)
                {
                    name = resourceName
                };
                m_ownedObjects.Add(texture);
                if (!s_loadImage(texture, buffer.ToArray(), false))
                    throw new InvalidOperationException($"Could not decode visual asset: {resourceName}");
                texture.Apply(mipmaps, true);
                return texture;
            }
        }

        private void RegisterOverride(string path, UnityObject asset)
        {
            m_assets.SetRuntimeOverride(path, asset);
            m_overrides.Add(path, asset);
        }

        public void Dispose()
        {
            foreach (var entry in m_overrides)
            {
                if (m_assets.TryGetRuntimeOverride(entry.Key, out UnityObject current) && current == entry.Value)
                    m_assets.RemoveRuntimeOverride(entry.Key);
            }
            // Sprite cache entries reference the owned icon textures across game sessions.
            // Clear them before releasing the textures so the next load builds fresh sprites.
            if (m_overrides.Count > 0)
                m_assets.ClearCachedAssets();
            m_overrides.Clear();
            foreach (UnityObject asset in m_ownedObjects)
                UnityObject.Destroy(asset);
            m_ownedObjects.Clear();
        }
    }
}
