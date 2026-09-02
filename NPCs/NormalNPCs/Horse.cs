using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Tools.ClimateChange;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Projectiles.Enemy;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.NormalNPCs
{
    public class Horse : ModNPC
    {
        private int chargetimer = 0;
        private int basespeed = 1;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Earth Elemental");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = 0.4f,
                PortraitScale = 0.6f,
                PortraitPositionYOverride = -20f
            };
            value.Position.X += 28f;
            value.Position.Y -= 56f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("A man-made elemental run with clockwork mechanisms, records say its previous design resembled a horse.")
            });
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 3f;
            NPC.damage = 50;
            NPC.width = 230;
            NPC.height = 230;
            NPC.defense = 20;
			NPC.DR_NERD(0.1f);
            NPC.lifeMax = 3800;
            NPC.aiStyle = -1;
            AIType = -1;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
			NPC.buffImmune[BuffID.Frostburn] = false;
			NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
			NPC.buffImmune[BuffID.StardustMinionBleed] = false;
			NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
			NPC.buffImmune[BuffID.Oiled] = false;
			NPC.buffImmune[BuffID.BetsysCurse] = false;
			NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
			NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<AbyssalFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<DemonFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WarCleave>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 1, 50, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.rarity = 2;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<EarthElementalBanner>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(chargetimer);
            writer.Write(basespeed);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            chargetimer = reader.ReadInt32();
            basespeed = reader.ReadInt32();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !Main.hardMode || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || NPC.AnyNPCs(ModContent.NPCType<Horse>()))
            {
                return 0f;
            }
            return SpawnCondition.Cavern.Chance * 0.005f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 8)
            {
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
                NPC.frameCounter = 0;
            }
        }

        public override void OnKill()
        {
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<AridArtifact>(), 3);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<SlagMagnum>(), 4);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Aftershock>(), 4);
			DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<EarthenPike>(), 4);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 31, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
                NPC.width = 160;
                NPC.height = 160;
                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 31, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
				CalamityUtils.ExplosionGores(NPC.GetSource_FromThis(), NPC.Center, 3);
            }
        }

        public override bool PreAI()
        {
			NPC.TargetClosest(true);

			if (Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				if (NPC.velocity.Y < -2f)
					NPC.velocity.Y = -2f;
				NPC.velocity.Y += 0.1f;
				if (NPC.velocity.Y > 12f)
					NPC.velocity.Y = 12f;

				if (NPC.timeLeft > 60)
					NPC.timeLeft = 60;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.localAI[0] += 1f;
                if (NPC.localAI[0] >= 300f)
                {
                    NPC.localAI[0] = 0f;
                    SoundEngine.PlaySound(SoundID.NPCHit43, NPC.Center);
                    NPC.TargetClosest(true);
                    if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                    {
                        float num179 = 4f;
                        Vector2 value9 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                        float num180 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - value9.X;
                        float num181 = Math.Abs(num180) * 0.1f;
                        float num182 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - value9.Y - num181;
                        float num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
                        NPC.netUpdate = true;
                        num183 = num179 / num183;
                        num180 *= num183;
                        num182 *= num183;
                        int num184 = 30;
                        int num185 = ModContent.ProjectileType<EarthRockSmall>();
                        value9.X += num180;
                        value9.Y += num182;
                        for (int num186 = 0; num186 < 4; num186++)
                        {
                            num185 = Main.rand.NextBool(4) ? ModContent.ProjectileType<EarthRockBig>() : ModContent.ProjectileType<EarthRockSmall>();
                            num180 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - value9.X;
                            num182 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - value9.Y;
                            num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
                            num183 = num179 / num183;
                            num180 += (float)Main.rand.Next(-40, 41);
                            num182 += (float)Main.rand.Next(-40, 41);
                            num180 *= num183;
                            num182 *= num183;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), value9.X, value9.Y, num180, num182, num185, num184, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }

            if (Math.Abs(NPC.velocity.X) > 0.2)
                NPC.spriteDirection = NPC.direction;

            Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
            direction.Normalize();
            chargetimer += Main.expertMode ? 2 : 1;
			if (chargetimer >= 600)
			{
				direction *= 6f;
				NPC.velocity = direction;
				chargetimer = 0;
			}

            if (Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) > basespeed)
                NPC.velocity *= 0.985f;

            if (Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) <= basespeed * 1.15)
                NPC.velocity = direction * basespeed;

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
        }
    }
}
