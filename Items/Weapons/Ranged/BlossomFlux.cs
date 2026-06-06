using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class BlossomFlux : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blossom Flux");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Fires a stream of leaves\n" +
                "Right click to fire a spore orb that explodes into a cloud of spore gas\n" +
                "Revengeance drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 62;
            Item.useTime = 4;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0.15f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LeafArrow>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;
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
                Item.useTime = 25;
                Item.useAnimation = 25;
                Item.UseSound = SoundID.Item77;
            }
            else
            {
                Item.useTime = 2;
                Item.useAnimation = 16;
                Item.UseSound = SoundID.Item5;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SporeBomb>(), (int)((double)damage * 4.0), Item.knockBack * 60f, player.whoAmI, 0.0f, 0.0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<LeafArrow>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }
    }
}
