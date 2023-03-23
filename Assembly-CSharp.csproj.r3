﻿<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <LangVersion>8.0</LangVersion>
    <_TargetFrameworkDirectories>non_empty_path_generated_by_unity.rider.package</_TargetFrameworkDirectories>
    <_FullFrameworkReferenceAssemblyPaths>non_empty_path_generated_by_unity.rider.package</_FullFrameworkReferenceAssemblyPaths>
    <DisableHandlePackageFileConflicts>true</DisableHandlePackageFileConflicts>
  </PropertyGroup>
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProductVersion>10.0.20506</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <RootNamespace></RootNamespace>
    <ProjectGuid>{5b7a64c2-8ee4-129e-7477-0769e827ac4b}</ProjectGuid>
    <ProjectTypeGuids>{E097FAD1-6243-4DAD-9C02-E9B9EFC3FFC1};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <AssemblyName>Assembly-CSharp</AssemblyName>
    <TargetFrameworkVersion>v4.7.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <BaseDirectory>.</BaseDirectory>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>Temp\Bin\Debug\</OutputPath>
    <DefineConstants>UNITY_2021_1_10;UNITY_2021_1;UNITY_2021;UNITY_5_3_OR_NEWER;UNITY_5_4_OR_NEWER;UNITY_5_5_OR_NEWER;UNITY_5_6_OR_NEWER;UNITY_2017_1_OR_NEWER;UNITY_2017_2_OR_NEWER;UNITY_2017_3_OR_NEWER;UNITY_2017_4_OR_NEWER;UNITY_2018_1_OR_NEWER;UNITY_2018_2_OR_NEWER;UNITY_2018_3_OR_NEWER;UNITY_2018_4_OR_NEWER;UNITY_2019_1_OR_NEWER;UNITY_2019_2_OR_NEWER;UNITY_2019_3_OR_NEWER;UNITY_2019_4_OR_NEWER;UNITY_2020_1_OR_NEWER;UNITY_2020_2_OR_NEWER;UNITY_2020_3_OR_NEWER;UNITY_2021_1_OR_NEWER;PLATFORM_ARCH_64;UNITY_64;UNITY_INCLUDE_TESTS;USE_SEARCH_ENGINE_API;USE_QUICK_SEARCH_MODULE;SCENE_TEMPLATE_MODULE;ENABLE_AR;ENABLE_AUDIO;ENABLE_CACHING;ENABLE_CLOTH;ENABLE_EVENT_QUEUE;ENABLE_MICROPHONE;ENABLE_MULTIPLE_DISPLAYS;ENABLE_PHYSICS;ENABLE_TEXTURE_STREAMING;ENABLE_VIRTUALTEXTURING;ENABLE_UNET;ENABLE_LZMA;ENABLE_UNITYEVENTS;ENABLE_VR;ENABLE_WEBCAM;ENABLE_UNITYWEBREQUEST;ENABLE_WWW;ENABLE_CLOUD_SERVICES;ENABLE_CLOUD_SERVICES_COLLAB;ENABLE_CLOUD_SERVICES_COLLAB_SOFTLOCKS;ENABLE_CLOUD_SERVICES_ADS;ENABLE_CLOUD_SERVICES_USE_WEBREQUEST;ENABLE_CLOUD_SERVICES_CRASH_REPORTING;ENABLE_CLOUD_SERVICES_PURCHASING;ENABLE_CLOUD_SERVICES_ANALYTICS;ENABLE_CLOUD_SERVICES_UNET;ENABLE_CLOUD_SERVICES_BUILD;ENABLE_CLOUD_LICENSE;ENABLE_EDITOR_HUB_LICENSE;ENABLE_WEBSOCKET_CLIENT;ENABLE_DIRECTOR_AUDIO;ENABLE_DIRECTOR_TEXTURE;ENABLE_MANAGED_JOBS;ENABLE_MANAGED_TRANSFORM_JOBS;ENABLE_MANAGED_ANIMATION_JOBS;ENABLE_MANAGED_AUDIO_JOBS;INCLUDE_DYNAMIC_GI;ENABLE_MONO_BDWGC;ENABLE_SCRIPTING_GC_WBARRIERS;PLATFORM_SUPPORTS_MONO;RENDER_SOFTWARE_CURSOR;ENABLE_VIDEO;PLATFORM_STANDALONE;PLATFORM_STANDALONE_WIN;UNITY_STANDALONE_WIN;UNITY_STANDALONE;ENABLE_RUNTIME_GI;ENABLE_MOVIES;ENABLE_NETWORK;ENABLE_CRUNCH_TEXTURE_COMPRESSION;ENABLE_OUT_OF_PROCESS_CRASH_HANDLER;ENABLE_CLUSTER_SYNC;ENABLE_CLUSTERINPUT;PLATFORM_UPDATES_TIME_OUTSIDE_OF_PLAYER_LOOP;GFXDEVICE_WAITFOREVENT_MESSAGEPUMP;ENABLE_WEBSOCKET_HOST;ENABLE_MONO;NET_STANDARD_2_0;ENABLE_PROFILER;DEBUG;TRACE;UNITY_ASSERTIONS;UNITY_EDITOR;UNITY_EDITOR_64;UNITY_EDITOR_WIN;ENABLE_UNITY_COLLECTIONS_CHECKS;ENABLE_BURST_AOT;UNITY_TEAM_LICENSE;ENABLE_CUSTOM_RENDER_TEXTURE;ENABLE_DIRECTOR;ENABLE_LOCALIZATION;ENABLE_SPRITES;ENABLE_TERRAIN;ENABLE_TILEMAP;ENABLE_TIMELINE;ENABLE_LEGACY_INPUT_MANAGER;CSHARP_7_OR_LATER;CSHARP_7_3_OR_NEWER</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <NoWarn>0169,0649</NoWarn>
    <AllowUnsafeBlocks>False</AllowUnsafeBlocks>
    <TreatWarningsAsErrors>False</TreatWarningsAsErrors>
  </PropertyGroup>
  <PropertyGroup>
    <NoConfig>true</NoConfig>
    <NoStdLib>true</NoStdLib>
    <AddAdditionalExplicitAssemblyReferences>false</AddAdditionalExplicitAssemblyReferences>
    <ImplicitlyExpandNETStandardFacades>false</ImplicitlyExpandNETStandardFacades>
    <ImplicitlyExpandDesignTimeFacades>false</ImplicitlyExpandDesignTimeFacades>
  </PropertyGroup>
  <ItemGroup>
     <Compile Include="Assets\Tools\Debug\CD.cs" />
     <Compile Include="Assets\Tools\Debug\NewBehaviourScript.cs" />
     <Compile Include="Assets\Tools\Debug\NewDebugCategory.cs" />
     <Compile Include="Assets\Tools\Debug\DebugCategories.cs" />
     <Compile Include="Assets\Tools\Debug\CustomMonoBehaviour.cs" />
     <Reference Include="UnityEngine">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ARModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ARModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AccessibilityModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AccessibilityModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AndroidJNIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AndroidJNIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AnimationModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AnimationModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AssetBundleModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AssetBundleModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.AudioModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.AudioModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ClothModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ClothModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ClusterInputModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ClusterInputModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ClusterRendererModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ClusterRendererModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.CoreModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.CoreModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.CrashReportingModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.CrashReportingModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.DSPGraphModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.DSPGraphModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.DirectorModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.DirectorModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.GIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.GIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.GameCenterModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.GameCenterModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.GridModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.GridModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.HotReloadModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.HotReloadModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.IMGUIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.IMGUIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ImageConversionModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ImageConversionModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.InputModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.InputLegacyModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.InputLegacyModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.JSONSerializeModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.JSONSerializeModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.LocalizationModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.LocalizationModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ParticleSystemModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ParticleSystemModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.PerformanceReportingModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.PerformanceReportingModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.PhysicsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.PhysicsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.Physics2DModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.Physics2DModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ProfilerModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ProfilerModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.RuntimeInitializeOnLoadManagerInitializerModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.RuntimeInitializeOnLoadManagerInitializerModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.ScreenCaptureModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.ScreenCaptureModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.SharedInternalsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.SharedInternalsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.SpriteMaskModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.SpriteMaskModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.SpriteShapeModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.SpriteShapeModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.StreamingModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.StreamingModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.SubstanceModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.SubstanceModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.SubsystemsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.SubsystemsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TLSModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TLSModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TerrainModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TerrainModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TerrainPhysicsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TerrainPhysicsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TextCoreModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TextCoreModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TextRenderingModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TextRenderingModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.TilemapModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.TilemapModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UIElementsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UIElementsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UIElementsNativeModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UIElementsNativeModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UNETModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UNETModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UmbraModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UmbraModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityAnalyticsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityAnalyticsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityConnectModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityConnectModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityCurlModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityCurlModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityTestProtocolModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityTestProtocolModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityWebRequestModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityWebRequestModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityWebRequestAssetBundleModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityWebRequestAssetBundleModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityWebRequestAudioModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityWebRequestAudioModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityWebRequestTextureModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityWebRequestTextureModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UnityWebRequestWWWModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.UnityWebRequestWWWModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.VFXModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.VFXModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.VRModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.VRModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.VehiclesModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.VehiclesModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.VideoModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.VideoModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.VirtualTexturingModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.VirtualTexturingModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.WindModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.WindModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.XRModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEngine.XRModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.CoreModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.CoreModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.DeviceSimulatorModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.DeviceSimulatorModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.GraphViewModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.GraphViewModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.PackageManagerUIModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.PackageManagerUIModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.QuickSearchModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.QuickSearchModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.SceneTemplateModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.SceneTemplateModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UIBuilderModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.UIBuilderModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UIElementsModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.UIElementsModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UIElementsSamplesModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.UIElementsSamplesModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UIServiceModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.UIServiceModule.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UnityConnectModule">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/Managed/UnityEngine/UnityEditor.UnityConnectModule.dll</HintPath>
     </Reference>
     <Reference Include="Newtonsoft.Json">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/PackageCache/com.unity.nuget.newtonsoft-json@2.0.0/Runtime/Newtonsoft.Json.dll</HintPath>
     </Reference>
     <Reference Include="netstandard">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/ref/2.0.0/netstandard.dll</HintPath>
     </Reference>
     <Reference Include="Microsoft.Win32.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/Microsoft.Win32.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.AppContext">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.AppContext.dll</HintPath>
     </Reference>
     <Reference Include="System.Collections.Concurrent">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Collections.Concurrent.dll</HintPath>
     </Reference>
     <Reference Include="System.Collections">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Collections.dll</HintPath>
     </Reference>
     <Reference Include="System.Collections.NonGeneric">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Collections.NonGeneric.dll</HintPath>
     </Reference>
     <Reference Include="System.Collections.Specialized">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Collections.Specialized.dll</HintPath>
     </Reference>
     <Reference Include="System.ComponentModel">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ComponentModel.dll</HintPath>
     </Reference>
     <Reference Include="System.ComponentModel.EventBasedAsync">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ComponentModel.EventBasedAsync.dll</HintPath>
     </Reference>
     <Reference Include="System.ComponentModel.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ComponentModel.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.ComponentModel.TypeConverter">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ComponentModel.TypeConverter.dll</HintPath>
     </Reference>
     <Reference Include="System.Console">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Console.dll</HintPath>
     </Reference>
     <Reference Include="System.Data.Common">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Data.Common.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.Contracts">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.Contracts.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.Debug">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.Debug.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.FileVersionInfo">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.FileVersionInfo.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.Process">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.Process.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.StackTrace">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.StackTrace.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.TextWriterTraceListener">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.TextWriterTraceListener.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.Tools">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.Tools.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.TraceSource">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.TraceSource.dll</HintPath>
     </Reference>
     <Reference Include="System.Diagnostics.Tracing">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Diagnostics.Tracing.dll</HintPath>
     </Reference>
     <Reference Include="System.Drawing.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Drawing.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.Dynamic.Runtime">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Dynamic.Runtime.dll</HintPath>
     </Reference>
     <Reference Include="System.Globalization.Calendars">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Globalization.Calendars.dll</HintPath>
     </Reference>
     <Reference Include="System.Globalization">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Globalization.dll</HintPath>
     </Reference>
     <Reference Include="System.Globalization.Extensions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Globalization.Extensions.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.Compression">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.Compression.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.Compression.ZipFile">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.Compression.ZipFile.dll</HintPath>
     </Reference>
     <Reference Include="System.IO">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.FileSystem">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.FileSystem.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.FileSystem.DriveInfo">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.FileSystem.DriveInfo.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.FileSystem.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.FileSystem.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.FileSystem.Watcher">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.FileSystem.Watcher.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.IsolatedStorage">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.IsolatedStorage.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.MemoryMappedFiles">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.MemoryMappedFiles.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.Pipes">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.Pipes.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.UnmanagedMemoryStream">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.IO.UnmanagedMemoryStream.dll</HintPath>
     </Reference>
     <Reference Include="System.Linq">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Linq.dll</HintPath>
     </Reference>
     <Reference Include="System.Linq.Expressions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Linq.Expressions.dll</HintPath>
     </Reference>
     <Reference Include="System.Linq.Parallel">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Linq.Parallel.dll</HintPath>
     </Reference>
     <Reference Include="System.Linq.Queryable">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Linq.Queryable.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Http">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Http.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.NameResolution">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.NameResolution.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.NetworkInformation">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.NetworkInformation.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Ping">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Ping.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Requests">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Requests.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Security">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Security.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.Sockets">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.Sockets.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.WebHeaderCollection">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.WebHeaderCollection.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.WebSockets.Client">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.WebSockets.Client.dll</HintPath>
     </Reference>
     <Reference Include="System.Net.WebSockets">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Net.WebSockets.dll</HintPath>
     </Reference>
     <Reference Include="System.ObjectModel">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ObjectModel.dll</HintPath>
     </Reference>
     <Reference Include="System.Reflection">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Reflection.dll</HintPath>
     </Reference>
     <Reference Include="System.Reflection.Extensions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Reflection.Extensions.dll</HintPath>
     </Reference>
     <Reference Include="System.Reflection.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Reflection.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.Resources.Reader">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Resources.Reader.dll</HintPath>
     </Reference>
     <Reference Include="System.Resources.ResourceManager">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Resources.ResourceManager.dll</HintPath>
     </Reference>
     <Reference Include="System.Resources.Writer">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Resources.Writer.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.CompilerServices.VisualC">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.CompilerServices.VisualC.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Extensions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Extensions.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Handles">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Handles.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.InteropServices">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.InteropServices.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.InteropServices.RuntimeInformation">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.InteropServices.RuntimeInformation.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Numerics">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Numerics.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Serialization.Formatters">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Serialization.Formatters.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Serialization.Json">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Serialization.Json.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Serialization.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Serialization.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Serialization.Xml">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Runtime.Serialization.Xml.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Claims">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Claims.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Cryptography.Algorithms">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Cryptography.Algorithms.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Cryptography.Csp">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Cryptography.Csp.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Cryptography.Encoding">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Cryptography.Encoding.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Cryptography.Primitives">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Cryptography.Primitives.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Cryptography.X509Certificates">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Cryptography.X509Certificates.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.Principal">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.Principal.dll</HintPath>
     </Reference>
     <Reference Include="System.Security.SecureString">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Security.SecureString.dll</HintPath>
     </Reference>
     <Reference Include="System.Text.Encoding">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Text.Encoding.dll</HintPath>
     </Reference>
     <Reference Include="System.Text.Encoding.Extensions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Text.Encoding.Extensions.dll</HintPath>
     </Reference>
     <Reference Include="System.Text.RegularExpressions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Text.RegularExpressions.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.Overlapped">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.Overlapped.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.Tasks">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.Tasks.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.Tasks.Parallel">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.Tasks.Parallel.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.Thread">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.Thread.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.ThreadPool">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.ThreadPool.dll</HintPath>
     </Reference>
     <Reference Include="System.Threading.Timer">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Threading.Timer.dll</HintPath>
     </Reference>
     <Reference Include="System.ValueTuple">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.ValueTuple.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.ReaderWriter">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.ReaderWriter.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.XDocument">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.XDocument.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.XmlDocument">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.XmlDocument.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.XmlSerializer">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.XmlSerializer.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.XPath">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.XPath.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.XPath.XDocument">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netstandard/System.Xml.XPath.XDocument.dll</HintPath>
     </Reference>
     <Reference Include="System.Numerics.Vectors">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/Extensions/2.0.0/System.Numerics.Vectors.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.InteropServices.WindowsRuntime">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/Extensions/2.0.0/System.Runtime.InteropServices.WindowsRuntime.dll</HintPath>
     </Reference>
     <Reference Include="mscorlib">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/mscorlib.dll</HintPath>
     </Reference>
     <Reference Include="System.ComponentModel.Composition">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.ComponentModel.Composition.dll</HintPath>
     </Reference>
     <Reference Include="System.Core">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Core.dll</HintPath>
     </Reference>
     <Reference Include="System.Data">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Data.dll</HintPath>
     </Reference>
     <Reference Include="System">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.dll</HintPath>
     </Reference>
     <Reference Include="System.Drawing">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Drawing.dll</HintPath>
     </Reference>
     <Reference Include="System.IO.Compression.FileSystem">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.IO.Compression.FileSystem.dll</HintPath>
     </Reference>
     <Reference Include="System.Net">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Net.dll</HintPath>
     </Reference>
     <Reference Include="System.Numerics">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Numerics.dll</HintPath>
     </Reference>
     <Reference Include="System.Runtime.Serialization">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Runtime.Serialization.dll</HintPath>
     </Reference>
     <Reference Include="System.ServiceModel.Web">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.ServiceModel.Web.dll</HintPath>
     </Reference>
     <Reference Include="System.Transactions">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Transactions.dll</HintPath>
     </Reference>
     <Reference Include="System.Web">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Web.dll</HintPath>
     </Reference>
     <Reference Include="System.Windows">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Windows.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Xml.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.Linq">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Xml.Linq.dll</HintPath>
     </Reference>
     <Reference Include="System.Xml.Serialization">
     <HintPath>C:/Unity/2021.1.10f1/Editor/Data/NetStandard/compat/2.0.0/shims/netfx/System.Xml.Serialization.dll</HintPath>
     </Reference>
     <Reference Include="Unity.VSCode.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.VSCode.Editor.dll</HintPath>
     </Reference>
     <Reference Include="Unity.TextMeshPro.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.TextMeshPro.Editor.dll</HintPath>
     </Reference>
     <Reference Include="Unity.VisualStudio.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.VisualStudio.Editor.dll</HintPath>
     </Reference>
     <Reference Include="Unity.Timeline.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.Timeline.Editor.dll</HintPath>
     </Reference>
     <Reference Include="Unity.Timeline">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.Timeline.dll</HintPath>
     </Reference>
     <Reference Include="Unity.TextMeshPro">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.TextMeshPro.dll</HintPath>
     </Reference>
     <Reference Include="UnityEditor.UI">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/UnityEditor.UI.dll</HintPath>
     </Reference>
     <Reference Include="Unity.PlasticSCM.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.PlasticSCM.Editor.dll</HintPath>
     </Reference>
     <Reference Include="Unity.Rider.Editor">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/Unity.Rider.Editor.dll</HintPath>
     </Reference>
     <Reference Include="UnityEngine.UI">
     <HintPath>D:/School/Capstone/I4F03-21C/trunk/4F03 Capstone - Base/Library/ScriptAssemblies/UnityEngine.UI.dll</HintPath>
     </Reference>
  </ItemGroup>
  <ItemGroup>
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it.
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>
