using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class DeathValley : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Death Valley Duster");
/*
            Tooltip.SetDefault("Casts a large blast of dust");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 110;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DustProjectile>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Tradewinds>());
            recipe.AddIngredient(ItemID.FossilOre, 25);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddIngredient(ModContent.ItemType<DesertFeather>(), 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
