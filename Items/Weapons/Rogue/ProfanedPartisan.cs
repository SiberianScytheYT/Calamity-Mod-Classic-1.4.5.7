using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class ProfanedPartisan : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Partisan");
/*
            Tooltip.SetDefault("Fires an unholy spear that explodes on death\n"
                               +"Stealth strikes spawn smaller spears to fly along side it");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 231;
            Item.knockBack = 8f;
            Item.crit += 15;

            Item.width = 56;
            Item.height = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.UseSound = SoundID.Item1;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise; //12
            Item.Calamity().rogue = true;

            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<ProfanedPartisanproj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {

            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CrystalPiercer>(), 200);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 4);
            recipe.AddIngredient(ModContent.ItemType <UnholyEssence>(), 25);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
