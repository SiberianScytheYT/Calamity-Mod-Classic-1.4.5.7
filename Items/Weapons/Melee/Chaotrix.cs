using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Chaotrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fault Line");
/*
            Tooltip.SetDefault("Explodes on enemy hits\n" +
			"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 88; // 110 pre-nerf
            Item.knockBack = 4f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            
            Item.shoot = ModContent.ProjectileType<ChaotrixYoyo>();
            Item.shootSpeed = 14f;

            Item.rare = 8;
            Item.value = Item.buyPrice(gold: 80);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
