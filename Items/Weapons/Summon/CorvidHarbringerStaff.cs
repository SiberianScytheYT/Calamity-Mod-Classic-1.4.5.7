using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class CorvidHarbringerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corvid Harbinger Staff");
/*
            Tooltip.SetDefault("Nevermore.\n" +
                               "Summons a powerful raven which teleports and dashes");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 52;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_BetsyFlyingCircleAttack;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 11;
            Item.damage = 300;
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<PowerfulRaven>();
            Item.shootSpeed = 13f;

            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RavenStaff);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
