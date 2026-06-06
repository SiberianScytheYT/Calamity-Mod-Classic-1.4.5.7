using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ScarletDevil : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scarlet Devil");
/*
            Tooltip.SetDefault("Throws an ultra high velocity spear, which creates more projectiles that home in\n" +
            "The spear creates a Scarlet Blast upon hitting an enemy\n" +
            "Stealth strikes grant you lifesteal\n" +
            "'Divine Spear \"Spear the Gungnir\"'");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 94;
            Item.height = 94;
            Item.damage = 40000;
            Item.crit += 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 60;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<ScarletDevilProjectile>();
            Item.shootSpeed = 30f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ProfanedTrident>());
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 15);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
