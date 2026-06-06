using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class CrystalPiercer : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crystal Piercer");
/*
            Tooltip.SetDefault("Throws a crystal javelin that pierces infinitely\n" +
			"Stealth strikes travel through blocks, ignore gravity, and summon crystal shards as they fly");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 62;
            Item.damage = 52;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 17;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.maxStack = 999;
            Item.value = 2500;
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<CrystalPiercerProjectile>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].aiStyle = -1;
                Main.projectile[stealth].tileCollide = false;
                Main.projectile[stealth].usesLocalNPCImmunity = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(20);
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
