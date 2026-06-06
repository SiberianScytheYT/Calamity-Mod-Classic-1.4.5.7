using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Accessories;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class StellarCulex : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stellar Culex");
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/StellarCulexGlow").Value;
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 50;
            NPC.aiStyle = 14; //bats
            NPC.npcSlots = 0.5f; //needed?
            NPC.damage = 55;
            NPC.defense = 18;
			NPC.DR_NERD(0.15f);
            NPC.knockBackResist = 0.65f;
            NPC.lifeMax = 210;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            AnimationType = NPCID.GiantFlyingFox;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<StellarCulexBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            NPC.buffImmune[BuffID.Confused] = false;
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 90;
                NPC.defense = 28;
                NPC.knockBackResist = 0.55f;
                NPC.lifeMax = 320;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //DO DUST
            int frame = NPC.frame.Y / frameHeight;
            Dust d = CalamityGlobalNPC.SpawnDustOnNPC(NPC, 100, frameHeight, ModContent.DustType<AstralOrange>(), new Rectangle(66, 10, (frame == 0 || frame == 3) ? 32 : 24, 16), Vector2.Zero, 0.45f, true);
            if (d != null)
            {
                d.customData = 0.04f;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 15;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit"), NPC.Center);
                        break;
                    case 1:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit2"), NPC.Center);
                        break;
                    case 2:
                        SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/NPCHit/AstralEnemyHit3"), NPC.Center);
                        break;
                }
            }

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, (Main.rand.Next(0, Math.Max(0, NPC.life)) == 0) ? 5 : ModContent.DustType<AstralEnemy>(), 1f, 4, 22);

            //if dead do gores
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity * 0.3f, Mod.Find<ModGore>("StellarCulexGore" + i).Type);
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(50, 30f);
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition, NPC.frame, Color.White * 0.6f, NPC.rotation, origin, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(2))
            {
                return 0.16f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItemChance(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), 0.5f, 1, 2);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), Main.expertMode);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<StarbusterCore>(), CalamityWorld.downedAstrageldon, 7, 1, 1);
        }
    }
}
