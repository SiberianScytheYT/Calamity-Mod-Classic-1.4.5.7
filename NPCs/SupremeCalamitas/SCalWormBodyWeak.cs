using CalRD.Projectiles.Boss;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.NPCs.SupremeCalamitas
{
    public class SCalWormBodyWeak : ModNPC
    {
		private bool setAlpha = false;

		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Heart");
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.npcSlots = 5f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.lifeMax = CalamityWorld.revenge ? 1200000 : 1000000;
            if (CalamityWorld.death)
            {
                NPC.lifeMax = 2000000;
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            CalamityGlobalNPC global = NPC.Calamity();
            global.DR = 0.999999f;
            global.unbreakableDR = true;
            NPC.scale = 1.2f;
            if (Main.expertMode)
            {
                NPC.scale = 1.35f;
            }
            NPC.alpha = 255;
            NPC.chaseable = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath13;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.dontCountMe = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
			writer.Write(setAlpha);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
			setAlpha = reader.ReadBoolean();
		}

        public override void AI()
        {
			if (NPC.ai[3] > 0f)
			{
				NPC.realLife = (int)NPC.ai[3];
			}

			bool flag = false;
			if (NPC.ai[1] <= 0f)
			{
				flag = true;
			}
			else if (Main.npc[(int)NPC.ai[1]].life <= 0 || NPC.life <= 0)
			{
				flag = true;
			}
			if (flag)
			{
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.checkDead();
			}

			if (Main.npc[(int)NPC.ai[1]].alpha < 128 && !setAlpha)
			{
				if (NPC.alpha != 0)
				{
					for (int num934 = 0; num934 < 2; num934++)
					{
						int num935 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 182, 0f, 0f, 100, default, 2f);
						Main.dust[num935].noGravity = true;
						Main.dust[num935].noLight = true;
					}
				}
				NPC.alpha -= 42;
				if (NPC.alpha <= 0)
				{
					setAlpha = true;
					NPC.alpha = 0;
				}
			}
			else
			{
				NPC.alpha = Main.npc[(int)NPC.ai[2]].alpha;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.localAI[0] += 1f;
				if (NPC.localAI[0] >= 900f)
				{
					NPC.localAI[0] = 0f;
					int type = ModContent.ProjectileType<BrimstoneBarrage>();
					int damage = NPC.GetProjectileDamage(type);
					int totalProjectiles = 4;
					float radians = MathHelper.TwoPi / totalProjectiles;
					Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
					for (int k = 0; k < totalProjectiles; k++)
					{
						Vector2 velocity = spinningPoint.RotatedBy(radians * k);
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, type, damage, 0f, Main.myPlayer);
					}
					SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
				}
			}

			Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			float num191 = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
			float num192 = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);
			num191 = (int)(num191 / 16f) * 16;
			num192 = (int)(num192 / 16f) * 16;
			vector18.X = (int)(vector18.X / 16f) * 16;
			vector18.Y = (int)(vector18.Y / 16f) * 16;
			num191 -= vector18.X;
			num192 -= vector18.Y;
			float num193 = (float)System.Math.Sqrt(num191 * num191 + num192 * num192);
			if (NPC.ai[1] > 0f && NPC.ai[1] < Main.npc.Length)
			{
				try
				{
					vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
					num191 = Main.npc[(int)NPC.ai[1]].position.X + (Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
					num192 = Main.npc[(int)NPC.ai[1]].position.Y + (Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
				}
				catch
				{
				}
				NPC.rotation = (float)System.Math.Atan2(num192, num191) + 1.57f;
				num193 = (float)System.Math.Sqrt(num191 * num191 + num192 * num192);
				int num194 = NPC.width;
				num193 = (num193 - num194) / num193;
				num191 *= num193;
				num192 *= num193;
				NPC.velocity = Vector2.Zero;
				NPC.position.X = NPC.position.X + num191;
				NPC.position.Y = NPC.position.Y + num192;
				if (num191 < 0f)
				{
					NPC.spriteDirection = -1;
				}
				else if (num191 > 0f)
				{
					NPC.spriteDirection = 1;
				}
			}
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
			if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => projectile.type != x))
			{
				if (projectile.penetrate == -1 && !projectile.minion)
				{
					projectile.penetrate = 1;
				}
				else if (projectile.penetrate >= 1)
				{
					projectile.penetrate = 1;
				}
			}
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
