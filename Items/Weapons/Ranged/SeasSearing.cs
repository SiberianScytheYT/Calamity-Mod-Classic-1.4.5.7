using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SeasSearing : ModItem
    {
        public static int BaseDamage = 60;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sea's Searing");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Fires a string of bubbles summoning a shower of bubbles on hit\n" +
                "Right click to fire a slower, larger water blast that summons a water spout\n" +
                "Revengeance drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 88;
            Item.height = 44;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = 5;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SeasSearingBubble>();
            Item.shootSpeed = 11f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -5);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
				Item.reuseDelay = 0;
            }
            else
            {
                Item.useTime = 5;
                Item.useAnimation = 15;
				Item.reuseDelay = 20;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SeasSearingSecondary>(), (int)(damage * 1.22f), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SeasSearingBubble>(), damage, Item.knockBack, player.whoAmI, 1f, 0f);
            }
			return false;
        }
    }
}
