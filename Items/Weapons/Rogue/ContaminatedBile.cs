using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ContaminatedBile : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Contaminated Bile");
/*
            Tooltip.SetDefault("Throws a flask of sickly green, irradiated bile which explodes on collision\n" +
                               "Stealth strikes make the explosion much more violent and powerful");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 9;
            Item.width = Item.height = 24;
            Item.useAnimation = Item.useTime = 31;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.rare = 2;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ContaminatedBileFlask>();
            Item.shootSpeed = 15f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            int p = Projectile.NewProjectile(source, position, velocity1, type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[p].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 10);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}
