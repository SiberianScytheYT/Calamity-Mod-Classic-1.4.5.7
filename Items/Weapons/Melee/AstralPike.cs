using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class AstralPike : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Pike");
/*
            Tooltip.SetDefault("Summons astral star swarms on critical hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.damage = 128;
            Item.crit += 25;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 13;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 50;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<AstralPikeProj>();
            Item.shootSpeed = 13f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
    }
}
