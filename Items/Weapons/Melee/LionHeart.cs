using CalRD.Projectiles.Ranged;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class LionHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lion Heart");
/*
            Tooltip.SetDefault("Summons an energy explosion on enemy hits\n" +
			"Right click to summon an energy shell for a few seconds that halves all damage sources\n" +
			"This has a 45 second cooldown");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 62;

            Item.damage = 700;
            Item.knockBack = 5.5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.shootSpeed = 0f;

            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;

            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ModContent.ProjectileType<EnergyShell>();
			}
			else
			{
				Item.shoot = ProjectileID.None;
			}
			return base.CanUseItem(player);
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.altFunctionUse == 2)
			{
				if (!player.Calamity().energyShellCooldown && player.ownedProjectileCounts[ModContent.ProjectileType<EnergyShell>()] <= 0)
				{
					Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<EnergyShell>(), 0, 0f, player.whoAmI, 0f, 0f);
				}
			}
            return false;
        }

		public override bool? CanHitNPC(Player player, NPC target)
		{
			if (player.altFunctionUse == 2)
			{
				return false;
			}
			return null;
		}

		public override bool CanHitPvp(Player player, Player target) => player.altFunctionUse != 2;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			int explosion = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<PlanarRipperExplosion>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[explosion].Calamity().forceMelee = true;
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			int explosion = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<PlanarRipperExplosion>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[explosion].Calamity().forceMelee = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 132);
            }
        }
    }
}
