using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TruePaladinsHammerMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fallen Paladin's Hammer");
/*
            Tooltip.SetDefault("Explodes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<OPHammer>();
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PaladinsHammer);
            recipe.AddIngredient(ModContent.ItemType<PwnagehammerMelee>());
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
