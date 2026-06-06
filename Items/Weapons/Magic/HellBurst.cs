using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class HellBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hell Burst");
/*
            Tooltip.SetDefault("Casts a beam of flame");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 31;
            Item.useAnimation = 31;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FlameBeamTip>();
            Item.shootSpeed = 32f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Flamelash);
            recipe.AddIngredient(ItemID.CrystalVileShard);
            recipe.AddIngredient(ItemID.DarkShard, 2);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
