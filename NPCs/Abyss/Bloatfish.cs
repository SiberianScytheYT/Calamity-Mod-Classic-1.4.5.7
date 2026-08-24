using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Ranged;
using CalRD.World;
using System;
using Terraria;
    using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRD.NPCs.Abyss
{
    public class Bloatfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloatfish");
            Main.npcFrameCount[NPC.type] = 4;
        }
        
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				new FlavorTextBestiaryInfoElement("Large fish which can easily resist the crushing pressure of the depths and any efforts to pierce their skin.")
			});
		}
        
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.damage = 5;
            NPC.width = 74;
            NPC.height = 94;
            NPC.defense = 100;
            NPC.lifeMax = 12000;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 30, 0);
            NPC.buffImmune[ModContent.BuffType<CrushDepth>()] = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.9f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<BloatfishBanner>();
        }

        public override void AI()
        {
            NPC.spriteDirection = (NPC.direction > 0) ? 1 : -1;
            NPC.noGravity = true;
            if (NPC.direction == 0)
            {
                NPC.TargetClosest(true);
            }
            if (NPC.wet)
            {
                NPC.TargetClosest(false);
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.velocity.X * -1f;
                    NPC.direction *= -1;
                    NPC.netUpdate = true;
                }
                if (NPC.collideY)
                {
                    NPC.netUpdate = true;
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                        NPC.directionY = -1;
                        NPC.ai[0] = -1f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                        NPC.directionY = 1;
                        NPC.ai[0] = 1f;
                    }
                }
                NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.1f;
                if (NPC.velocity.X < -0.2f || NPC.velocity.X > 0.2f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.95f;
                }
                if (NPC.ai[0] == -1f)
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                    if ((double)NPC.velocity.Y < -0.3)
                    {
                        NPC.ai[0] = 1f;
                    }
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                    if ((double)NPC.velocity.Y > 0.3)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                int num258 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int num259 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                if (Main.tile[num258, num259 - 1].LiquidAmount > 128)
                {
                    if (Main.tile[num258, num259 + 1].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                    else if (Main.tile[num258, num259 + 2].HasTile)
                    {
                        NPC.ai[0] = -1f;
                    }
                }
                if ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.95f;
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.94f;
                    if ((double)NPC.velocity.X > -0.2 && (double)NPC.velocity.X < 0.2)
                    {
                        NPC.velocity.X = 0f;
                    }
                }
                NPC.velocity.Y = NPC.velocity.Y + 0.3f;
                if (NPC.velocity.Y > 3f)
                {
                    NPC.velocity.Y = 3f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if ((double)NPC.rotation < -0.2)
            {
                NPC.rotation = -0.2f;
            }
            if ((double)NPC.rotation > 0.2)
            {
                NPC.rotation = 0.2f;
                return;
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if ((projectile.penetrate == -1 || projectile.penetrate > 1) && !projectile.minion)
            {
                projectile.penetrate = 1;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 900, true);
        }

        public override void FindFrame(int frameHeight)
        {
            if (!NPC.wet)
            {
                NPC.frameCounter = 0.0;
                return;
            }
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneAbyssLayer4 && spawnInfo.Water && NPC.CountNPCS(ModContent.NPCType<Bloatfish>()) < 3)
            {
                return SpawnCondition.CaveJellyfish.Chance * 0.3f;
            }
            return 0f;
        }

        public override void OnKill()
        {
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<HalibutCannon>(), CalamityWorld.revenge, CalamityGlobalNPCLoot.halibutCannonBaseDropChance, 1, 1);
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Voidstone>(), 10, 20);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DepthCells>(), CalamityWorld.downedCalamitas, 2, 5, 7);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<DepthCells>(), CalamityWorld.downedCalamitas && Main.expertMode, 2, 2, 3);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
            if (NPC.scale < 2f)
            {
                NPC.scale += 0.05f;
            }
        }
    }
}
