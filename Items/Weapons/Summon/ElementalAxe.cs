using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class ElementalAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Axe");
/*
            Tooltip.SetDefault("Summons an elemental axe to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 140;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ElementalAxeMinion>();
            Item.shootSpeed = 10f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                position = Main.MouseWorld;
                velocity.X = 0;
                velocity.Y = 0;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddIngredient(ModContent.ItemType<PlantationStaff>());
            recipe.AddIngredient(ModContent.ItemType<HauntedScroll>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
