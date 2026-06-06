using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TriactisTruePaladinianMageHammerofMightMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Triactis' True Paladinian Mage-Hammer of Might");
/*
            Tooltip.SetDefault("Explodes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 160;
            Item.damage = 10000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.knockBack = 50f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 160;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<TriactisOPHammer>();
            Item.shootSpeed = 25f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalaxySmasherMelee>());
            recipe.AddIngredient(ItemID.SoulofMight, 30);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
