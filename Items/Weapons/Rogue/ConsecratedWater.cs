using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ConsecratedWater : RogueWeapon
    {
        public const int BaseDamage = 48;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Consecrated Water");
/*
            Tooltip.SetDefault("The bottle is surprisingly dusty\n" +
							   "Throws a holy flask of water that explodes into a sacred flame pillar on death\n" +
                               "The pillar is destroyed if there's no tiles below it\n" +
                               "Stealth strikes create three flame pillars instead of one on impact");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.width = 22;
            Item.height = 24;
            Item.useAnimation = 29;
            Item.useTime = 29;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.rare = 6;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 48); //sell price of 9 gold 60 silver
            Item.shoot = ModContent.ProjectileType<ConsecratedWaterProjectile>();
            Item.shootSpeed = 15f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            float strikeValue = player.Calamity().StealthStrikeAvailable().ToInt(); //0 if false, 1 if true
            int p = Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<ConsecratedWaterProjectile>(), damage, Item.knockBack, player.whoAmI, ai1: strikeValue);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HolyWater, 100);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 20);
            recipe.AddIngredient(ItemID.SoulofLight, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
