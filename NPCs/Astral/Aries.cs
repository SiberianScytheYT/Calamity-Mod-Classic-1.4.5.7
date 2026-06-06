using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Banners;
using CalRD.Items.Weapons.Rogue;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRD.NPCs.Astral
{
    public class Aries : ModNPC
    {
        private static Texture2D glowmask;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aries");
            Main.npcFrameCount[NPC.type] = 8;
            if (!Main.dedServ)
                glowmask = ModContent.Request<Texture2D>("CalRD/NPCs/Astral/AriesGlow").Value;
        }

        public override void SetDefaults()
        {
            NPC.damage = 50;
            NPC.width = 56;
            NPC.height = 54;
            NPC.aiStyle = 41;
            NPC.defense = 14;
			NPC.DR_NERD(0.15f);
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0.6f;
            NPC.value = Item.buyPrice(0, 0, 10, 0);
            NPC.DeathSound = new SoundStyle("CalRD/Sounds/NPCKilled/AstralEnemyDeath");
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AriesBanner>();
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            if (CalamityWorld.downedAstrageldon)
            {
                NPC.damage = 85;
                NPC.defense = 24;
                NPC.knockBackResist = 0.5f;
                NPC.lifeMax = 450;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            CalamityGlobalNPC.SpawnDustOnNPC(NPC, 66, frameHeight, ModContent.DustType<AstralOrange>(), new Rectangle(44, 18, 12, 12));
            if (NPC.velocity.Y == 0f)
            {
                NPC.frame.Y = 0;
            }
            else if ((double)NPC.velocity.Y < -1.5)
            {
                NPC.frame.Y = frameHeight * 7;
            }
            else if ((double)NPC.velocity.Y < 0)
            {
                NPC.frame.Y = frameHeight * 4;
            }
            else if ((double)NPC.velocity.Y > 1.5)
            {
                NPC.frame.Y = frameHeight * 6;
            }
            else
            {
                NPC.frame.Y = frameHeight * 5;
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

            CalamityGlobalNPC.DoHitDust(NPC, hit.HitDirection, ModContent.DustType<AstralOrange>(), 1f, 4, 24);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //draw glowmask
            spriteBatch.Draw(glowmask, NPC.Center - Main.screenPosition, NPC.frame, Color.White * 0.6f, NPC.rotation, new Vector2(33, 31), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CalamityGlobalNPC.AnyEvents(spawnInfo.Player))
            {
                return 0f;
            }
            else if (spawnInfo.Player.InAstral(1))
            {
                return 0.15f;
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
            DropHelper.DropItemCondition(NPC.GetSource_FromThis(), NPC, ModContent.ItemType<StellarKnife>(), CalamityWorld.downedAstrageldon, 7, 1, 1);
        }
    }
}
