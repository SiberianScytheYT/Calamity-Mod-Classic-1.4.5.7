using CalRD.Projectiles.Summon;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class MidnightSunBeacon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Midnight Sun Beacon");
/*
            Tooltip.SetDefault("Summons a UFO to vaporize enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 240;
            Item.mana = 12;
            Item.width = Item.height = 32;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item90;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MidnightSunBeaconProj>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.XenoStaff);
            recipe.AddIngredient(ItemID.MoonlordTurretStaff);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 25);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 25);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
