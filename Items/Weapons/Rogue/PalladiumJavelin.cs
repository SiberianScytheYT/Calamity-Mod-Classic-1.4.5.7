using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class PalladiumJavelin : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Palladium Javelin");
/*
            Tooltip.SetDefault("Stealth strikes split into more javelins");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 44;
            Item.damage = 50;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 19;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.shoot = ProjectileID.StarAnise;
            Item.maxStack = 999;
            Item.value = 1200;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<PalladiumJavelinProjectile>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int javelin = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[javelin].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
			if (!player.Calamity().StealthStrikeAvailable())
				Main.projectile[javelin].usesLocalNPCImmunity = false;
			return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(20);
            recipe.AddIngredient(ItemID.PalladiumBar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
