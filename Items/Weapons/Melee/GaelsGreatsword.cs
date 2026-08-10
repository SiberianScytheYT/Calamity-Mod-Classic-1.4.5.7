using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalRD.Items.Weapons.Melee
{
	public class GaelsGreatsword : ModItem
    {
        //Help, they're forcing me to slave away at Calamity until I die! - Dominic

        //Weapon attribute constants

        public static readonly int BaseDamage = 3900;

        public static readonly float TrueMeleeBoost = 2.5f;

        public static readonly float GiantSkullDamageMultiplier = 1.5f;

        //Weapon projectile attribute constants

        public static readonly int SearchDistance = 1450;

        public static readonly int ImmunityFrames = 2;

        public static readonly int SkullsplosionCooldownSeconds = 30;

        //Skull ring attribute constants

        public static readonly float MaxRageBoost = 1.5f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gael's Greatsword");
/*
            Tooltip.SetDefault("Hand it over, that thing. Your dark soul.\n" +
							   "First swing fires homing skulls\n" +
                               "Second swing fires a giant, powerful skull\n" +
                               "Third swing deals massive damage\n" +
                               "Constantly generates rage when in use\n" +
                               "Swings leave behind exploding blood trails when below 50% health\n" +
                               "Right click to swipe the sword, reflecting projectiles at a 50% chance\n" +
                               "Activating Rage Mode releases an enormous barrage of skulls");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        //NOTE: GetWeaponDamage is in the CalamityPlayer file
        public override void SetDefaults()
        {
            Item.width = 88;
            Item.height = 84;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 12;
            Item.useTurn = true;
            Item.knockBack = 9;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<GaelSkull>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        public override void HoldItem(Player player)
        {
            player.endurance += 0.1f; //10% DR boost
        }
        public override bool AltFunctionUse(Player player) => true;
        public override Vector2? HoldoutOffset() => new Vector2(12, 12);

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return (12f/46f);
			return 1f;
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (CalamityUtils.CountProjectiles(ModContent.ProjectileType<LightningThing>()) < 3 &&
                player.statLife <= player.statLifeMax2 * 0.5f)
            {
                Point origin = (player.Center + Main.rand.Next(-300, 301) * Vector2.UnitX).ToTileCoordinates();
                Point p;
                if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(400), new GenCondition[]
                {
                    new Conditions.IsSolid()
                }), out p))
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), p.ToWorldCoordinates(8f, 0f), Vector2.Zero, ModContent.ProjectileType<LightningThing>(), 0, 0f, player.whoAmI);
                }
            }
            if (player.itemAnimation == (int)(player.itemAnimationMax * 0.5))
            {
                player.Calamity().gaelSwipes++;
                if (player.statLife <= player.statLifeMax2 * 0.5f)
                {
                    for (int i = 0; i < 170; i++)
                    {
                        float r = (float)Math.Sqrt(Main.rand.NextDouble());
                        float t = Main.rand.NextFloat() * MathHelper.TwoPi;
                        Vector2 dustSpawn = t.ToRotationVector2() * r * Item.Size;
                        if (dustSpawn.X > Item.width / 2)
                        {
                            Dust.NewDustPerfect(player.MountedCenter + dustSpawn.RotatedBy(player.itemRotation) * player.direction, 218, Vector2.Zero).noGravity = true;
                        }
                        else
                        {
                            //Don't waste this version of "i" just because we failed. Decrease so that we can try again.
                            i--;
                            continue;
                        }
                        if (Main.rand.NextBool(100))
                        {
                            Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.MountedCenter + dustSpawn.RotatedBy(player.itemRotation) * player.direction,
                                                     Vector2.Zero,
                                                     ModContent.ProjectileType<GaelExplosion>(),
                                                     (int)(Item.damage * player.MeleeDamage()),
                                                     0f,
                                                     player.whoAmI);
                        }
                    }
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = CalRD.Instance.GetPacket();
                    netMessage.Write((byte)CalRDMessageType.GaelsGreatswordSwingSync);
                    netMessage.Write((byte)player.whoAmI);
                    netMessage.Write(player.Calamity().gaelSwipes);
                    netMessage.Send();
                }
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //True melee boost
            if (player.Calamity().gaelSwipes % 3 == 2)
            {
                hit.Damage *= (int)TrueMeleeBoost;
            }
        }
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            //True melee boost
            if (player.Calamity().gaelSwipes % 3 == 2)
            {
                hurtInfo.Damage *= (int)TrueMeleeBoost;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                // Check CalamityPlayer.cs
                return false;
            }
            switch (player.Calamity().gaelSwipes % 3)
            {
                //Two small, quick skulls
                case 0:
                    int numProj = 2;
                    float rotation = MathHelper.ToRadians(10f);
                    for (int i = 0; i < numProj; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                        Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, Item.knockBack, player.whoAmI);
                    }
                    break;
                //Giant, slow, fading skull
                case 1:
					int largeSkullDmg = (int)(damage * 1.5f);
                    int projectileIndex = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y) * 0.5f, type, largeSkullDmg, Item.knockBack, player.whoAmI, ai1:1f);
                    Main.projectile[projectileIndex].scale = 1.75f;
                    break;
            }
            return false;
        }
    }
}
