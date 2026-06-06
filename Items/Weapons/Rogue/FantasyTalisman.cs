using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class FantasyTalisman : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fantasy Talisman");
/*
            Tooltip.SetDefault("Fires high velocity talismans that ignore gravity\n"
                               +"Talismans attach to enemies, causing them to release lost souls\n"
                               +"Stealth strikes release more souls and leave behind souls as they travel");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.damage = 93;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 60, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<FantasyTalismanProj>();
            Item.shootSpeed = 18f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<FantasyTalismanStealth>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(150);
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 2);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
