using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class SunGodStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sun God Staff");
/*
            Tooltip.SetDefault("Summons a solar god spirit to protect you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.mana = 10;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.25f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<SolarGod>();
            Item.DamageType = DamageClass.Summon;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SunSpiritStaff>());
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddTile(TileID.MythrilAnvil);
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
