using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class PrimordialAncient : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Primordial Ancient");
/*
            Tooltip.SetDefault("An ancient relic from an ancient land\n" +
                "Casts a gigantic blast of dust");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 305;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 36;
            Item.height = 48;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Ancient>();
            Item.shootSpeed = 8f;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PrimordialEarth>());
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 5);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
