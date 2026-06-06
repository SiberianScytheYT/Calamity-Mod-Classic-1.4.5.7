using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class DeificThunderbolt : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Deific Thunderbolt");
/*
            Tooltip.SetDefault("Fires a lightning bolt to electrocute enemies\n"
                               +"The lightning bolt travels faster while it is raining\n"
                               +"Summons lightning from the sky on impact\n"
                               +"Stealth strikes summon more lightning and travel faster");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 567;
            Item.knockBack = 10f;
            Item.crit += 12;

            Item.width = 56;
            Item.height = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen; //13
            Item.Calamity().rogue = true;

            Item.autoReuse = true;
            Item.shootSpeed = 13.69f;
            Item.shoot = ModContent.ProjectileType<DeificThunderboltProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float stealthSpeedMult = 1f;
			if (player.Calamity().StealthStrikeAvailable())
				stealthSpeedMult = 1.5f;
			float rainSpeedMult = 1f;
			if (Main.raining)
				rainSpeedMult = 1.5f;

			int thunder = Projectile.NewProjectile(source, position.X, position.Y, velocity.X * rainSpeedMult * stealthSpeedMult, velocity.Y * rainSpeedMult * stealthSpeedMult, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
			{
				Main.projectile[thunder].Calamity().stealthStrike = true;
			}
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StormfrontRazor>());
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 8);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 15);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
