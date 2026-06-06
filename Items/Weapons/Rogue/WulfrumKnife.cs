using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class WulfrumKnife : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Knife");
/*
            Tooltip.SetDefault("Stealth strikes make the knife fly further and hit several times at once");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 22;
            Item.damage = 11;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 38;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = 1;
            Item.shoot = ModContent.ProjectileType<WulfrumKnifeProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y) * 1.3f, ModContent.ProjectileType<WulfrumKnifeProj>(), damage, Item.knockBack, player.whoAmI);
                Projectile proj = Main.projectile[p];
                proj.Calamity().stealthStrike = true;
                proj.penetrate = 4;
                proj.usesLocalNPCImmunity = true;
                proj.localNPCHitCooldown = 1;
                return false;
            }
            return true;
        }
    }
}
