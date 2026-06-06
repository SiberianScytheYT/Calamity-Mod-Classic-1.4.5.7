using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class SunSpiritStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sun Spirit Staff");
/*
            Tooltip.SetDefault("Summons a solar spirit to protect you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.mana = 10;
            Item.width = 44;
            Item.height = 42;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.15f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<SolarPixie>();
            Item.DamageType = DamageClass.Summon;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SandstoneBrick, 20);
            recipe.AddIngredient(ModContent.ItemType<DesertFeather>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(true, type, player);
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            return false;
        }
    }
}
