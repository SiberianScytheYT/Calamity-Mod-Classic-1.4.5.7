using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class YinYo : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yin-Yo");
/*
            Tooltip.SetDefault("Fires light or dark shards when enemies are near\n" +
                "Shards fly back and forth\n" +
				"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 34;
            Item.knockBack = 3.5f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<YinYoyo>();
            Item.shootSpeed = 12f;

            Item.rare = 5;
            Item.value = Item.buyPrice(gold: 36);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DarkShard);
            recipe.AddIngredient(ItemID.LightShard);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
