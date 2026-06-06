using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class StarofDestruction : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star of Destruction");
/*
            Tooltip.SetDefault("Fires a huge destructive mine that explodes into destruction bolts\n" +
            "Amount of bolts scales with enemies hit, up to 16\n" +
            "Stealth strikes always explode into the max amount of bolts");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = Item.height = 94;
            Item.damage = 150;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 38;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<DestructionStar>();
            Item.shootSpeed = 5f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeldiateBar>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
