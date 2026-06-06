using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Magic;
using CalRD.Projectiles.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TenebreusTides : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tenebreus Tides");
/*
            Tooltip.SetDefault("Inundatio ex Laminis\n" +
			"Shoots a water spear that pierces enemies and terrain\n" +
			"Striking enemies summon liquid blades and spears to assault the struck foe");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.knockBack = 4.5f;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 14;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TenebreusTidesProjectile>();
            Item.shootSpeed = 12f;

            Item.width = Item.height = 72;
            Item.useStyle = 5;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.value = CalamityGlobalItem.Rarity9BuyPrice;
            Item.rare = 9;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AmidiasTrident>());
            recipe.AddIngredient(ModContent.ItemType<Atlantis>());
            recipe.AddIngredient(ItemID.InfluxWaver);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 25);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 50);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
