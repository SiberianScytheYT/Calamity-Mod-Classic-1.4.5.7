using CalRD.Projectiles.Summon;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class TacticalPlagueEngine : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tactical Plague Engine");
/*
            Tooltip.SetDefault("Summons a plague jet to pummel your enemies into submission\n" +
                               "Consumes bullets\n" +
                               "Sometimes shoots a missile instead of a bullet");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.mana = 10;
            Item.width = 28;
            Item.height = 20;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item14;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Summon;
            Item.shoot = ModContent.ProjectileType<TacticalPlagueEngineSummon>();
            Item.shootSpeed = 16f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BlackHawkRemote>());
            recipe.AddIngredient(ModContent.ItemType<InfectedRemote>());
            recipe.AddIngredient(ModContent.ItemType<FuelCellBundle>());
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 15);
			recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 8);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
