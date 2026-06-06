using CalRD.CalPlayer;
using CalRD.Projectiles.Healing;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
    public class FungalClumpMinion : ModProjectile
    {
		private bool returnToPlayer = false;
		private bool amalgam = false;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fungal Clump");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();

            bool correctMinion = Projectile.type == ModContent.ProjectileType<FungalClumpMinion>();
            if (!modPlayer.fungalClump)
            {
                Projectile.active = false;
                return;
            }
            if (correctMinion)
            {
                if (player.dead)
                {
                    modPlayer.fClump = false;
                }
                if (modPlayer.fClump)
                {
                    Projectile.timeLeft = 2;
                }
            }
			amalgam = Projectile.ai[0] == 1f;

			//Initializing dust and damage
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                int num226 = 36;
                for (int num227 = 0; num227 < num226; num227++)
                {
                    Vector2 vector6 = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
                    vector6 = vector6.RotatedBy((double)((float)(num227 - (num226 / 2 - 1)) * 6.28318548f / (float)num226), default) + Projectile.Center;
                    Vector2 vector7 = vector6 - Projectile.Center;
                    int num228 = Dust.NewDust(vector6 + vector7, 0, 0, 56, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 1.4f);
                    Main.dust[num228].noGravity = true;
                    Main.dust[num228].noLight = true;
                    Main.dust[num228].velocity = vector7;
                }
                Projectile.localAI[0] += 1f;
            }

			//Flexible damage correction
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int damage2 = (int)((float)Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = damage2;
            }

			//Periodically create dust
            if (Main.rand.NextBool(16))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 56, Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f);
            }

			//Anti-sticky movement failsafe
			Projectile.MinionAntiClump();

			//If summoned by Amalgam, trail poisonous seawater
			if (Math.Abs(Projectile.velocity.X) > 0.1f || Math.Abs(Projectile.velocity.Y) > 0.1f)
            {
                if (Projectile.owner == Main.myPlayer && amalgam)
                {
                    int water = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<PoisonousSeawater>(), Projectile.damage, 0f, Projectile.owner, 1f, 0f);
					Main.projectile[water].usesIDStaticNPCImmunity = true;
					Main.projectile[water].usesLocalNPCImmunity = false;
                }
            }

			//If too far from player, increase speed to chase after player
			float playerRange = 500f;
			//Range is boosted if chasing after an enemy
			if (Projectile.ai[1] != 0f || Projectile.friendly)
				playerRange = amalgam ? 2000f : 1400f;
			if (Math.Abs(Projectile.Center.X - player.Center.X) + Math.Abs(Projectile.Center.Y - player.Center.Y) > playerRange)
				returnToPlayer = true;

			//Find an npc to target, or if minion targetting is used, choose that npc
			Vector2 targetVec = Projectile.Center;
			float range = 900f;
			bool npcFound = false;
            Vector2 half = new Vector2(0.5f);
			if (!returnToPlayer)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					NPC npc = Main.npc[player.MinionAttackTargetNPC];
					if (npc.CanBeChasedBy(Projectile, false))
					{
						//Check the size of the target to make it easier to hit fat targets like Levi
						Vector2 sizeCheck = npc.position + npc.Size * half;
						float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
						//Some minions will ignore tiles when choosing a target like Ice Claspers, others will not
						bool canHit = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						if (!npcFound && targetDist < range && canHit)
						{
							range = targetDist;
							targetVec = sizeCheck;
							npcFound = true;
						}
					}
				}
				//If no npc is specifically targetted, check through the entire array
				if (!npcFound)
				{
					for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
					{
						NPC npc = Main.npc[npcIndex];
						if (npc.CanBeChasedBy(Projectile, false))
						{
							Vector2 sizeCheck = npc.position + npc.Size * half;
							float targetDist = Vector2.Distance(npc.Center, Projectile.Center);
							bool canHit = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
							if (!npcFound && targetDist < range && canHit)
							{
								range = targetDist;
								targetVec = sizeCheck;
								npcFound = true;
							}
						}
					}
				}
			}

			//Tile collision depends on if returning to the player or not
			Projectile.tileCollide = !returnToPlayer;

			if (!npcFound)
			{
				Projectile.friendly = true;
				float homingSpeed = amalgam ? 12f : 8f;
				float turnSpeed = 20f;
				if (returnToPlayer) //move faster if returning to the player
					homingSpeed = amalgam ? 30f : 12f;
				Vector2 playerVector = player.Center - Projectile.Center;
				playerVector.Y -= 60f;
				float playerDist = playerVector.Length();
				if (playerDist < 100f && returnToPlayer && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					returnToPlayer = false;
				if (playerDist > 2000f)
				{
					Projectile.position.X = player.Center.X - Projectile.width / 2;
					Projectile.position.Y = player.Center.Y - Projectile.width / 2;
				}
				//If more than 70 pixels away, move toward the player
                if (playerDist > 70f)
                {
                    playerVector.Normalize();
                    playerVector *= homingSpeed;
                    Projectile.velocity = (Projectile.velocity * turnSpeed + playerVector) / (turnSpeed + 1f);
                }
				//Minions never stay still
                else
                {
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
                }
				Projectile.friendly = false;
				Projectile.rotation = Projectile.velocity.X * 0.05f;
				if (Math.Abs(Projectile.velocity.X) <= 0f)
					return;
				Projectile.spriteDirection = -Projectile.direction;
			}
			else
			{
				if (Projectile.ai[1] == -1f)
					Projectile.ai[1] = 17f;
				if (Projectile.ai[1] > 0f)
				{
					Projectile.ai[1] -= 1f;
				}
				if (Projectile.ai[1] == 0f)
				{
					Projectile.friendly = true;
					float minionSpeed = amalgam ? 20f : 8f;
					float turnSpeed = 14f;
					Vector2 targetLocation = targetVec - Projectile.Center;
					float targetDist = targetLocation.Length();
					if (targetDist < 100f)
						minionSpeed = amalgam ? 25f : 10f;

					Vector2 homeInVector = Projectile.DirectionTo(targetLocation);
					if (homeInVector.HasNaNs())
						homeInVector = Vector2.UnitY;

                    targetLocation.Normalize();
                    targetLocation *= minionSpeed;
                    Projectile.velocity = (Projectile.velocity * turnSpeed + targetLocation) / (turnSpeed + 1f);
				}
				else
				{
					Projectile.friendly = false;
					if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 10f)
						Projectile.velocity *= 1.05f;
				}
				Projectile.rotation = Projectile.velocity.X * 0.05f;
				if (Math.Abs(Projectile.velocity.X) <= 0.2f)
					return;
				Projectile.spriteDirection = -Projectile.direction;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy || !target.canGhostHeal)
            {
                return;
            }
            float healAmt = Projectile.damage * 0.25f;
            if ((int)healAmt == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
			if (healAmt > 50f)
				healAmt = 50f;
			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], healAmt, ModContent.ProjectileType<FungalHeal>(), 1200f, 1f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}
