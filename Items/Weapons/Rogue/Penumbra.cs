using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class Penumbra : RogueWeapon
    {
        public static float ShootSpeed = 8f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Penumbra");
/*
            Tooltip.SetDefault("Throws a shadow bomb that explodes into homing souls \n" +
                               "Stealth strikes make the bomb manifest on the cursor and explode into more souls");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 46;
            Item.height = 32;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item103;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;

            Item.damage = 1600;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.crit = 16;
            Item.knockBack = 8f;
            Item.shoot = ModContent.ProjectileType<PenumbraBomb>();
            Item.shootSpeed = ShootSpeed;

            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {    
            if (player.Calamity().StealthStrikeAvailable())
            {
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                float num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
                float num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (player.gravDir == -1f)
                {
                    num79 = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
                }
                if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
                {
                    num78 = player.direction;
                    num79 = 0f;
                }
                vector2 += new Vector2(num78, num79);
                int proj = Projectile.NewProjectile(source, vector2, new Vector2(0f,-0.5f), ModContent.ProjectileType<PenumbraBomb>(), damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 24);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 6);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
