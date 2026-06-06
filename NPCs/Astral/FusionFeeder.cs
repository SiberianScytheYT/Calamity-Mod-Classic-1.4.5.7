using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class FusionFeeder : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fusion Feeder");
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/FusionFeederGlow").Value;
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.width = 120;
            NPC.height = 24;
            NPC.damage = 45;
            NPC.aiStyle = 103;
            NPC.lifeMax = 400;
            NPC.defense = 12;
			NPC.DR_NERD(0.15f);
            NPC.value = Item.buyPrice(0, 0, 20, 0);
            NPC.knockBackResist = 0.8f;
            NPC.behindTiles = true;
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            AnimationType = NPCID.SandShark;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FusionFeederBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 65;
                NPC.defense = 22;
                NPC.knockBackResist = 0.7f;
                NPC.lifeMax = 600;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            //DO DUST
            Dust d = CalamityGlobalNPC.SpawnDustOnNPC(NPC, 134, frameHeight, ModContent.DustType<AstralOrange>(), new Rectangle(46, 4, 60, 6), Vector2.Zero, 0.55f, true);
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

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, (Main.rand.Next(0, Math.Max(0, NPC.life)) == 0) ? 5 : ModContent.DustType<AstralEnemy>(), 1f, 4, 25);

            //if dead do gores
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        float rand = Main.rand.NextFloat(-0.18f, 0.18f);
                        Gore.NewGore(NPC.GetSource_FromThis(), NPC.position + new Vector2(Main.rand.NextFloat(0f, NPC.width), Main.rand.NextFloat(0f, NPC.height)), NPC.velocity * rand, Mod.Find<ModGore>("FusionFeederGore" + i).Type);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 offset = new Vector2(0f, 10f);
            Vector2 origin = new Vector2(67f, 23f);

            //draw shark
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + offset, NPC.frame, drawColor, NPC.rotation, origin, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            //draw glowmask
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition + offset, NPC.frame, Color.White * 0.6f, NPC.rotation, origin, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(3))
            {
                return 0.14f;
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120, true);
        }

        public override void OnKill()
        {
            DropHelper.DropItem(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), 2, 3);
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<Stardust>(), Main.expertMode);
        }
    }
}
