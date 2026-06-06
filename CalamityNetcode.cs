using CalRD.Events;
using CalRD.NPCs;
using CalRD.NPCs.NormalNPCs;
using CalRD.NPCs.OldDuke;
using CalRD.NPCs.Providence;
using CalRD.TileEntities;
using CalRD.World;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD
{
    public class CalamityNetcode
    {
        public static void HandlePacket(Mod mod, BinaryReader reader, int whoAmI)
        {
            try
            {
                CalRDMessageType msgType = (CalRDMessageType)reader.ReadByte();
                switch (msgType)
                {
                    case CalRDMessageType.MeleeLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleLevels(reader, 0);
                        break;
                    case CalRDMessageType.RangedLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleLevels(reader, 1);
                        break;
                    case CalRDMessageType.MagicLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleLevels(reader, 2);
                        break;
                    case CalRDMessageType.SummonLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleLevels(reader, 3);
                        break;
                    case CalRDMessageType.RogueLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleLevels(reader, 4);
                        break;
                    case CalRDMessageType.ExactMeleeLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleExactLevels(reader, 0);
                        break;
                    case CalRDMessageType.ExactRangedLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleExactLevels(reader, 1);
                        break;
                    case CalRDMessageType.ExactMagicLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleExactLevels(reader, 2);
                        break;
                    case CalRDMessageType.ExactSummonLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleExactLevels(reader, 3);
                        break;
                    case CalRDMessageType.ExactRogueLevelSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleExactLevels(reader, 4);
                        break;
                    case CalRDMessageType.StressSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleStress(reader);
                        break;
                    case CalRDMessageType.BossRushStage:
                        int stage = reader.ReadInt32();
                        BossRushEvent.BossRushStage = stage;
                        break;
                    case CalRDMessageType.AdrenalineSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleAdrenaline(reader);
                        break;
                    case CalRDMessageType.TeleportPlayer:
                        Main.player[reader.ReadInt32()].Calamity().HandleTeleport(reader.ReadInt32(), true, whoAmI);
                        break;
                    case CalRDMessageType.DoGCountdownSync:
                        int countdown = reader.ReadInt32();
                        CalamityWorld.DoGSecondStageCountdown = countdown;
                        break;
                    case CalRDMessageType.BossSpawnCountdownSync:
                        int countdown2 = reader.ReadInt32();
                        CalamityWorld.bossSpawnCountdown = countdown2;
                        break;
                    case CalRDMessageType.BRHostileProjKillSync:
                        int countdown3 = reader.ReadInt32();
                        CalamityWorld.bossRushHostileProjKillCounter = countdown3;
                        break;
                    case CalRDMessageType.DeathBossSpawnCountdownSync:
                        int countdown4 = reader.ReadInt32();
                        CalamityWorld.deathBossSpawnCooldown = countdown4;
                        break;
                    case CalRDMessageType.ArmoredDiggerCountdownSync:
                        int countdown5 = reader.ReadInt32();
                        CalamityWorld.ArmoredDiggerSpawnCooldown = countdown5;
                        break;
                    case CalRDMessageType.BossTypeSync:
                        int type = reader.ReadInt32();
                        CalamityWorld.bossType = type;
                        break;
                    case CalRDMessageType.DeathCountSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleDeathCount(reader);
                        break;
                    case CalRDMessageType.DeathModeUnderworldTimeSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleDeathModeUnderworldTime(reader);
                        break;
                    case CalRDMessageType.DeathModeBlizzardTimeSync:
                        Main.player[reader.ReadInt32()].Calamity().HandleDeathModeBlizzardTime(reader);
                        break;
                    case CalRDMessageType.NPCRegenerationSync:
                        byte npcIndex = reader.ReadByte();
                        Main.npc[npcIndex].lifeRegen = reader.ReadInt32();
                        break;
                    case CalRDMessageType.AcidRainSync:
                        CalamityWorld.rainingAcid = reader.ReadBoolean();
                        CalamityWorld.acidRainPoints = reader.ReadInt32();
                        CalamityWorld.timeSinceAcidRainKill = reader.ReadInt32();
                        break;
                    case CalRDMessageType.AcidRainUIDrawFadeSync:
                        CalamityWorld.acidRainExtraDrawTime = reader.ReadInt32();
                        break;
                    case CalRDMessageType.AcidRainOldDukeSummonSync:
                        CalamityWorld.triedToSummonOldDuke = reader.ReadBoolean();
                        break;
					case CalRDMessageType.EncounteredOldDukeSync:
						CalamityWorld.encounteredOldDuke = reader.ReadBoolean();
						break;
					case CalRDMessageType.GaelsGreatswordSwingSync:
                        byte playerIndex = reader.ReadByte();
                        Main.player[playerIndex].Calamity().gaelSwipes = reader.ReadInt32();
                        break;
                    case CalRDMessageType.SpawnSuperDummy:
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        // Not strictly necessary, but helps prevent unnecessary packetstorm in MP
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            NPC.NewNPC(new EntitySource_WorldEvent(),x, y, ModContent.NPCType<SuperDummyNPC>());
                        break;


                    //
                    // Ozzatron's packets
                    //
                    case CalRDMessageType.PowerCellFactory:
                        TEPowerCellFactory.ReadSyncPacket(mod, reader);
                        break;
                    case CalRDMessageType.ChargingStationStandard:
                        TEChargingStation.ReadSyncPacket(mod, reader);
                        break;
                    case CalRDMessageType.ChargingStationItemChange:
                        TEChargingStation.ReadItemSyncPacket(mod, reader);
                        break;
                    case CalRDMessageType.Turret:
                        TEBaseTurret.ReadSyncPacket(mod, reader);
                        break;
                    case CalRDMessageType.LabHologramProjector:
                        TELabHologramProjector.ReadSyncPacket(mod, reader);
                        break;
                    // This code has been edited to fail gracefully when trying to provide data for an invalid NPC.
                    case CalRDMessageType.SyncCalamityNPCAIArray:
                        // Read the entire packet regardless of anything
                        byte npcIdx = reader.ReadByte();
                        float ai0 = reader.ReadSingle();
                        float ai1 = reader.ReadSingle();
                        float ai2 = reader.ReadSingle();
                        float ai3 = reader.ReadSingle();

                        // If the NPC in question isn't valid, don't do anything.
                        NPC npc = Main.npc[npcIdx];
                        if (!npc.active)
                            break;

                        CalamityGlobalNPC cgn = npc.Calamity();
                        cgn.newAI[0] = ai0;
                        cgn.newAI[1] = ai1;
                        cgn.newAI[2] = ai2;
                        cgn.newAI[3] = ai3;
                        break;


                    case CalRDMessageType.ProvidenceDyeConditionSync:
                        byte npcIndex3 = reader.ReadByte();
                        (Main.npc[npcIndex3].ModNPC as Providence).hasTakenDaytimeDamage = reader.ReadBoolean();
                        break;
                    case CalRDMessageType.PSCChallengeSync:
                        byte npcIndex4 = reader.ReadByte();
                        (Main.npc[npcIndex4].ModNPC as Providence).challenge = reader.ReadBoolean();
                        break;

                    case CalRDMessageType.ServersideSpawnOldDuke:
                        byte playerIndex2 = reader.ReadByte();

                        if (Main.netMode != NetmodeID.Server)
                            break;

                        Player player = Main.player[playerIndex2];
                        if (!player.active || player.dead)
                            return;

                        Projectile projectile = null;
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            projectile = Main.projectile[i];
                            if (Main.projectile[i].active && Main.projectile[i].bobber && Main.projectile[i].owner == playerIndex2)
                            {
                                projectile = Main.projectile[i];
                                break;
                            }
                        }

                        if (projectile is null)
                            return;

                        int oldDuke = NPC.NewNPC(new EntitySource_WorldEvent(), (int)projectile.Center.X, (int)projectile.Center.Y + 100, ModContent.NPCType<OldDuke>());
                        CalamityUtils.BossAwakenMessage(oldDuke);
                        break;

                    default:
                        CalRD.Instance.Logger.Error($"Failed to parse Calamity packet: No Calamity packet exists with ID {msgType}.");
                        break;
                }
            }
            catch (Exception e)
            {
                if (e is EndOfStreamException eose)
                    CalRD.Instance.Logger.Error("Failed to parse Calamity packet: Packet was too short, missing data, or otherwise corrupt.", eose);
                else if (e is ObjectDisposedException ode)
                    CalRD.Instance.Logger.Error("Failed to parse Calamity packet: Packet reader disposed or destroyed.", ode);
                else if (e is IOException ioe)
                    CalRD.Instance.Logger.Error("Failed to parse Calamity packet: An unknown I/O error occurred.", ioe);
                else
                    throw e; // this either will crash the game or be caught by TML's packet policing
            }
        }

        public static void SyncWorld()
        {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
    }

    public enum CalRDMessageType : byte
    {
        MeleeLevelSync,
        RangedLevelSync,
        MagicLevelSync,
        SummonLevelSync,
        RogueLevelSync,

        ExactMeleeLevelSync,
        ExactRangedLevelSync,
        ExactMagicLevelSync,
        ExactSummonLevelSync,
        ExactRogueLevelSync,

        StressSync,
        AdrenalineSync,

        TeleportPlayer,
        BossRushStage,
        DoGCountdownSync,
        BossSpawnCountdownSync,
        BRHostileProjKillSync,
        ArmoredDiggerCountdownSync,
        BossTypeSync,
        DeathCountSync,

        NPCRegenerationSync,

        DeathModeUnderworldTimeSync,
        DeathModeBlizzardTimeSync,
        DeathBossSpawnCountdownSync,

        AcidRainSync,
        AcidRainUIDrawFadeSync,
        AcidRainOldDukeSummonSync,
		EncounteredOldDukeSync,

		GaelsGreatswordSwingSync,

        SpawnSuperDummy,
        SyncCalamityNPCAIArray,

        ProvidenceDyeConditionSync, // We shouldn't fucking need this. Die in a hole, Multiplayer.
        PSCChallengeSync, // See above

        // These message types were written by Ozz. They are Ozz's working tile entity netcode. Do not touch them.
        PowerCellFactory,
        ChargingStationStandard,
        ChargingStationItemChange,
        Turret,
        LabHologramProjector,

        ServersideSpawnOldDuke
    }
}
