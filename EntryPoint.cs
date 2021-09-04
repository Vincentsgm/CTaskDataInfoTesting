using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Attributes;

[assembly: Rage.Attributes.Plugin("CTaskDataInfoTesting", EntryPoint = "CTaskDataInfoTesting.EntryPoint.Main", PrefersSingleInstance = true)]

namespace CTaskDataInfoTesting
{
    internal static unsafe class EntryPoint
    {
        static IntPtr addr;
        static CTaskDataInfoManager* taskDataMgr;

        public static unsafe void Main()
        {
            addr = Game.FindPattern("48 8B 0D ?? ?? ?? ?? 4A 8B 04 C1 44 39 10 74 16");
            if (addr == IntPtr.Zero) Game.UnloadActivePlugin();
            addr += *(int*)(addr + 3) + 7;
            taskDataMgr = (CTaskDataInfoManager*)addr;
            for (int i = 0; i < taskDataMgr->aTaskData.Count; i++)
            {
                CTaskDataInfo* taskData = taskDataMgr->aTaskData.Items[i];
                if (taskData->Name == Game.GetHashKey("standard_ped"))
                {
                    Game.LogTrivial("Found CTaskDataInfo for standard_ped");
                    taskData->Flags |= eTaskDataInfoFlags.DisableWallHitAnimation;
                }
            }

            while (true)
            {
                GameFiber.Yield();
            }
        }

        [ConsoleCommand]
        public static void PrintGameHashKey()
        {
            Game.LogTrivial($"{Game.GetHashKey("standard_ped"):X8}");
        }

        [ConsoleCommand]
        public static unsafe void TestTaskData()
        {
            for (int i = 0; i < taskDataMgr->aTaskData.Count; i++)
            {
                CTaskDataInfo* taskData = taskDataMgr->aTaskData.Items[i];
                Game.LogTrivial($"Name = {taskData->Name:X8}, Flags = {taskData->Flags}");
            }
        }


        [StructLayout(LayoutKind.Sequential, Size = 0x10)]
        public unsafe struct atArray<T> where T : unmanaged
        {
            public T* Items;
            public ushort Count;
            public ushort Size;
        }

        [StructLayout(LayoutKind.Sequential, Size = 0x10)]
        public unsafe struct atArrayOfPtrs<T> where T : unmanaged
        {
            public T** Items;
            public ushort Count;
            public ushort Size;
        }

        [StructLayout(LayoutKind.Explicit, Size = 0x18)]
        struct CTaskDataInfoManager
        {
            [FieldOffset(0x00)] public atArrayOfPtrs<CTaskDataInfo> aTaskData;
            [FieldOffset(0x10)] public unsafe CTaskDataInfo* DefaultSet;
        }

        [StructLayout(LayoutKind.Explicit, Size = 0x20)]
        struct CTaskDataInfo
        {
            [FieldOffset(0x00)] public uint Name;
            [FieldOffset(0x04)] public uint TaskWanderConditionalAnimsGroup;
            [FieldOffset(0x08)] public float ScenarioAttractionDistance;
            [FieldOffset(0x0C)] public float SurfaceSwimmingDepthOffset;
            [FieldOffset(0x10)] public float SwimmingWanderPointRange;
            [FieldOffset(0x18)] public eTaskDataInfoFlags Flags;
        }

        [Flags]
        enum eTaskDataInfoFlags : ulong // real name hash is 0x982F0A37
        {
            ConsiderAsInWaterInLowLodPhysics = 1UL << 0,
            DisableCover = 1UL << 1,
            DisableCower = 1UL << 2,
            DisableHandsUp = 1UL << 3,
            DisableVehicleUsage = 1UL << 4,
            DisableNonStandardDamageTypes = 1UL << 5,
            DisableUseScenariosWithNoModelSet = 1UL << 6,
            DisableWallHitAnimation = 1UL << 7,
            DisableReactAndFleeAnimation = 1UL << 8,
            DisablePotentialBlastResponse = 1UL << 9,
            DisablePotentialToBeWalkedIntoResponse = 1UL << 10,
            PreferFleeOnPavements = 1UL << 11,
            DisableMeleeIntroAnimation = 1UL << 12,
            PlayNudgeAnimationForBumps = 1UL << 13,
            DontInfluenceWantedLevel = 1UL << 14,
            DisableBraceForImpact = 1UL << 15,
            CanScreamDuringFlee = 1UL << 16,
            IgnorePavementCheckWhenLeavingScenarios = 1UL << 17,
            DiesInstantlyToFire = 1UL << 18,
            UseAmbientScaling = 1UL << 19,
            CanBeShoved = 1UL << 20,
            DisableEvasiveStep = 1UL << 21,
            FleeFromCombatWhenTargetIsInVehicle = 1UL << 22,
            CanBeTalkedTo = 1UL << 23,
            AlwaysSprintWhenFleeingInThreatResponse = 1UL << 24,
            UseAquaticRoamMode = 1UL << 25,
            ApplyExtraHeadingChangesInFishLocomotion = 1UL << 26,
            AlignPitchToWavesInFishLocomotion = 1UL << 27,
            UseSimplePitchingInFishLocomotion = 1UL << 28,
            ProbeForCollisionsInScenarioFlee = 1UL << 29,
            SwimStraightInSwimmingWander = 1UL << 30,
            UseSimpleWander = 1UL << 31,
            BlockIdleTurnsInSmartFleeWhileWaitingOnPath = 1UL << 32,
            ForceSlowChaseInAnimalMelee = 1UL << 33,
            ExpandAvoidanceRadius = 1UL << 34,
            DependentAmbientFriend = 1UL << 35,
            UseLooseCrowdAroundMetrics = 1UL << 36,
            UseLooseHeadingAdjustmentsInMelee = 1UL << 37,
            ForceNoTurningInFishLocomotion = 1UL << 38,
            UseLongerBlendsInFishLocomotion = 1UL << 39,
            ForceSlowSwimWhenUnderPlayerControl = 1UL << 40,
            CanShove = 1UL << 41,
            BlockPlayerFishPitchingWhenSlow = 1UL << 42,
            UseSlowPlayerFishPitchAcceleration = 1UL << 43,
            IgnoreDelaysOnGunshotEvents = 1UL << 44,
            DiesInstantlyToMelee = 1UL << 45,
        }
    }
}
