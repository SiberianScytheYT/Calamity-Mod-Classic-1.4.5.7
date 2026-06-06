using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AegisBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aegis Blade");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Striking an enemy with the blade causes an earthen eruption\n" +
                "Right click to fire an aegis bolt\n" +
                "Revengeance drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
			Item.height = 58;
			Item.scale = 1.5f;
			Item.damage = 68;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = 7;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.shootSpeed = 14f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.UseSound = SoundID.Item73;
                Item.shoot = ModContent.ProjectileType<AegisBeam>();
            }
            else
            {
                Item.noMelee = false;
                Item.UseSound = SoundID.Item1;
                Item.shoot = ProjectileID.None;
            }
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse != 2)
				return 1f;
			//return 0.75f;
            return 1.33f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<AegisBeam>(), (int)(damage * 0.85), Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 246, 0f, 0f, 0, new Color(255, Main.DiscoG, 53));
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<AegisBlast>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<AegisBlast>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
        }
    }
}
