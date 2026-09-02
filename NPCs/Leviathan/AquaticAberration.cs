using CalRD.Buffs.StatDebuffs;
using CalRD.Events;
using CalRD.Items.Placeables.Banners;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
namespace CalRD.NPCs.Leviathan
{
    public class AquaticAberration : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Aberration");
            Main.npcFrameCount[NPC.type] = 7;
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("This creature devours whole schools of fish entirely by itself.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
			NPC.GetNPCDamage();
			NPC.width = 50;
            NPC.height = 50;
            NPC.defense = 14;
            NPC.lifeMax = 1600;
            if (BossRushEvent.BossRushActive)
            {
                NPC.lifeMax = 100000;
            }
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            AIType = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<AquaticAberrationBanner>();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            NPC.rotation = NPC.velocity.ToRotation();
            if (Math.Sign(NPC.velocity.X) != 0)
            {
                NPC.spriteDirection = -Math.Sign(NPC.velocity.X);
            }
            if (NPC.rotation < -MathHelper.PiOver2)
            {
                NPC.rotation += MathHelper.Pi;
            }
            if (NPC.rotation > MathHelper.PiOver2)
            {
                NPC.rotation -= MathHelper.Pi;
            }
            NPC.spriteDirection = Math.Sign(NPC.velocity.X);
            float num1000 = 30f;
            float num1006 = 0.111111117f * num1000;
            if (NPC.ai[0] == 0f)
            {
                float scaleFactor6 = 8f;
                Vector2 center4 = NPC.Center;
                Vector2 center5 = Main.player[NPC.target].Center;
                Vector2 vector126 = center5 - center4;
                Vector2 vector127 = vector126 - Vector2.UnitY * 300f;
                float num1013 = vector126.Length();
                vector126 = Vector2.Normalize(vector126) * scaleFactor6;
                vector127 = Vector2.Normalize(vector127) * scaleFactor6;
                bool flag64 = Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1);
                if (NPC.ai[3] >= 120f)
                {
                    flag64 = true;
                }
                float num1014 = 8f;
                flag64 = flag64 && vector126.ToRotation() > MathHelper.Pi / num1014 && vector126.ToRotation() < MathHelper.Pi - MathHelper.Pi / num1014;
                if (num1013 > 800f || !flag64)
                {
                    NPC.velocity.X = (NPC.velocity.X * (num1000 - 1f) + vector127.X) / num1000;
                    NPC.velocity.Y = (NPC.velocity.Y * (num1000 - 1f) + vector127.Y) / num1000;
                    if (!flag64)
                    {
                        NPC.ai[3] += 1f;
                        if (NPC.ai[3] == 120f)
                        {
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        NPC.ai[3] = 0f;
                    }
                }
                else
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[2] = vector126.X;
                    NPC.ai[3] = vector126.Y;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.velocity *= 0.8f;
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 5f)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                    Vector2 velocity = new Vector2(NPC.ai[2], NPC.ai[3]);
                    velocity.Normalize();
                    velocity *= 10f;
                    NPC.velocity = velocity;
                }
            }
            else if (NPC.ai[0] == 2f)
            {
                NPC.ai[1] += 1f;
				bool flag65 = NPC.Center.Y + 50f > Main.player[NPC.target].Center.Y;
				if ((NPC.ai[1] >= 90f && flag65) || NPC.velocity.Length() < 8f)
                {
					NPC.ai[0] = 3f;
					NPC.ai[1] = 45f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.velocity /= 2f;
                    NPC.netUpdate = true;
                }
                else
                {
                    Vector2 center6 = NPC.Center;
                    Vector2 center7 = Main.player[NPC.target].Center;
                    Vector2 vec2 = center7 - center6;
                    vec2.Normalize();
                    if (vec2.HasNaNs())
                    {
                        vec2 = new Vector2((float)NPC.direction, 0f);
                    }
                    NPC.velocity = (NPC.velocity * (num1000 - 1f) + vec2 * (NPC.velocity.Length() + num1006)) / num1000;
                }
            }
            else if (NPC.ai[0] == 3f)
            {
                NPC.ai[1] -= 1f;
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
                NPC.velocity *= 0.98f;
            }
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneSulphur || (!NPC.downedPlantBoss && !CalamityWorld.downedCalamitas))
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.02f;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Wet, 120, true);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
