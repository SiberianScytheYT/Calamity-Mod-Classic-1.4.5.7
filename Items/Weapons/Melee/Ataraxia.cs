using CalRD.Items.Materials;
using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Ataraxia : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ataraxia");
/*
            Tooltip.SetDefault("Equanimity");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 92;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 3651;
            Item.knockBack = 2.5f;
            Item.useAnimation = 19;
            Item.useTime = 19;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);

            Item.shoot = ModContent.ProjectileType<AtaraxiaMain>();
            Item.shootSpeed = 9f;
        }

        // Fires one large and two small projectiles which stay together in formation.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Play the Terra Blade sound upon firing
            SoundEngine.PlaySound(SoundID.Item60, position);

            // Center projectile
            int centerID = ModContent.ProjectileType<AtaraxiaMain>();
            int centerDamage = damage;
            Vector2 centerVel = new Vector2(velocity.X, velocity.Y);
            Projectile.NewProjectile(source, position, centerVel, centerID, centerDamage, Item.knockBack, player.whoAmI, 0f, 0f);

            // Side projectiles (these deal 75% damage)
            int sideID = ModContent.ProjectileType<AtaraxiaSide>();
            int sideDamage = (int)(0.75f * centerDamage);
            Vector2 speed = new Vector2(velocity.X, velocity.Y);
            speed.Normalize();
            speed *= 22f;
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 leftOffset = speed.RotatedBy(MathHelper.PiOver4, default);
            Vector2 rightOffset = speed.RotatedBy(-MathHelper.PiOver4, default);
            leftOffset -= 1.4f * speed;
            rightOffset -= 1.4f * speed;
            Projectile.NewProjectile(source, rrp.X + leftOffset.X, rrp.Y + leftOffset.Y, velocity.X, velocity.Y, sideID, sideDamage, Item.knockBack, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, rrp.X + rightOffset.X, rrp.Y + rightOffset.Y, velocity.X, velocity.Y, sideID, sideDamage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        // On-hit, tosses out five homing projectiles. This is not like Holy Collider.
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 480);
            target.AddBuff(BuffID.Ichor, 480);

            // Does not summon extra projectiles versus dummies.
            if (target.type == NPCID.TargetDummy)
                return;

			OnHitEffects(player, target.Center);
        }

        // On-hit, tosses out five homing projectiles. This is not like Holy Collider.
        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Shadowflame>(), 480);
            target.AddBuff(BuffID.Ichor, 480);
			OnHitEffects(player, target.Center);
        }

		private void OnHitEffects(Player player, Vector2 targetPos)
		{

            // Individual true melee homing missiles deal 10% of the weapon's base damage.
            int numSplits = 5;
            int trueMeleeID = ModContent.ProjectileType<AtaraxiaHoming>();
            int trueMeleeDamage = (int)(0.1f * Item.damage * player.MeleeDamage());
            float angleVariance = MathHelper.TwoPi / (float)numSplits;
            float spinOffsetAngle = MathHelper.Pi / (2f * numSplits);
            Vector2 posVec = new Vector2(8f, 0f).RotatedByRandom(MathHelper.TwoPi);

            for (int i = 0; i < numSplits; ++i)
            {
                posVec = posVec.RotatedBy(angleVariance);
                Vector2 velocity = new Vector2(posVec.X, posVec.Y).RotatedBy(spinOffsetAngle);
                velocity.Normalize();
                velocity *= 8f;
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), targetPos + posVec, velocity, trueMeleeID, trueMeleeDamage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
		}

        // Spawn some fancy dust while swinging
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustCount = Main.rand.Next(3, 6);
            Vector2 corner = new Vector2(hitbox.X + hitbox.Width / 4, hitbox.Y + hitbox.Height / 4);
            for (int i = 0; i < dustCount; ++i)
            {
                // Pick a random dust to spawn
                int dustID;
                switch (Main.rand.Next(5))
                {
                    case 0:
                    case 1:
                        dustID = 70;
                        break;
                    case 2:
                        dustID = 71;
                        break;
                    default:
                        dustID = 86;
                        break;
                }
                int idx = Dust.NewDust(corner, hitbox.Width / 2, hitbox.Height / 2, dustID);
                Main.dust[idx].noGravity = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ItemID.BrokenHeroSword);
			r.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12);
            r.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            r.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.Register();
        }
    }
}
